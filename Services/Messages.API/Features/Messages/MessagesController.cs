using AutoMapper;
using BuildingBlocks.Extensions;
using BuildingBlocks.Shared;
using Messages.API.Data;
using Messages.API.Extensions;
using Messages.API.Filters;
using Messages.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;

namespace Messages.API.Features.Messages;

[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
public class MessagesController(
    MessageContext context,
    IMapper mapper,
    IMessageRepository messageRepository,
    IUserRepository userRepository) : ControllerBase
{
    /// <summary>
    /// Create message
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessageAsync(CreateMessageRequestDto dto)
    {
        var username = User.GetUsername();

        if (username == dto.RecipientUsername.ToLower())
            return BadRequest("You cannot message yourself");

        var sender = await userRepository.GetUserByUsernameAsync(username!);
        var recipient = await userRepository.GetUserByUsernameAsync(dto.RecipientUsername);

        if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null)
            return BadRequest("Cannot send message at this time");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = dto.Content,
            FileUrl = dto.FileUrl,
        };

        await messageRepository.AddMessageAsync(message);

        if (await context.SaveChangesAsync() > 0)
        {
            return Ok(mapper.Map<MessageDto>(message));
        }

        return BadRequest("Failed to save message");
    }

    /// <summary>
    /// Get messages for the user
    /// </summary>
    /// <param name="messageParams"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUserAsync(
        [FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUsername()!;

        var messages = await messageRepository.GetMessagesForUserAsync(messageParams);

        Response.AddPaginationHeader(messages);

        return Ok(messages);
    }

    /// <summary>
    /// Get message by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThreadAsync(string username)
    {
        var messages = await messageRepository.GetMessageThreadAsync(User.GetUsername()!, username);
        return Ok(mapper.Map<IEnumerable<MessageDto>>(messages));
    }

    /// <summary>
    /// Delete message by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteMessageAsync(int id)
    {
        var username = User.GetUsername();

        var message = await messageRepository.GetMessageAsync(id);

        if (message == null) return BadRequest("Cannot delete this message");

        if (message.SenderUsername != username && message.RecipientUsername != username)
            return Forbid();

        if (message.SenderUsername == username) message.SenderDeleted = true;
        if (message.RecipientUsername == username) message.RecipientDeleted = true;

        if (message is { SenderDeleted: true, RecipientDeleted: true })
        {
            messageRepository.DeleteMessage(message);
        }

        if (await context.SaveChangesAsync() > 0) return Ok();

        return BadRequest("Problem deleting the message");
    }

    /// <summary>
    /// Count of new messages
    /// </summary>
    /// <returns></returns>
    [HttpGet("new-messages-count")]
    public async Task<ActionResult<int>> CountOfNewMessagesAsync()
    {
        var username = User.GetUsername();
        var count = await messageRepository.CountOfNewMessagesAsync(username!);
        return Ok(count);
    }
}