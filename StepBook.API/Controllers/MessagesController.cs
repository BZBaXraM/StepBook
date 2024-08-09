using StepBook.API.Contracts.Interfaces;

namespace StepBook.API.Controllers;

/// <summary>
/// Messages controller
/// </summary>
/// <param name="unitOfWork"></param>
/// <param name="mapper"></param>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessagesController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Create message
    /// </summary>
    /// <param name="messageCreateDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessageAsync(CreateMessageRequestDto messageCreateDto)
    {
        var username = User.GetUsername();

        if (username == messageCreateDto.RecipientUsername)
            return BadRequest("You cannot send messages to yourself");

        var sender = await unitOfWork.UserRepository.GetUserByUserNameAsync(username);
        var recipient = await unitOfWork.UserRepository.GetUserByUserNameAsync(messageCreateDto.RecipientUsername);

        if (recipient is null) return NotFound();

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = messageCreateDto.Content
        };

        unitOfWork.MessageRepository.AddMessage(message);

        if (await unitOfWork.MessageRepository.SaveAllAsync())
            return Ok(mapper.Map<MessageDto>(message));

        return BadRequest("Failed to send message");
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

        var messages = await unitOfWork.MessageRepository.GetMessageForUserAsync(messageParams);

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
        var messages = await unitOfWork.MessageRepository.GetMessageThreadAsync(User.GetUsername()!, username);
        return Ok(mapper.Map<IEnumerable<MessageDto>>(messages));
    }

    /// <summary>
    /// Delete message by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var message = await unitOfWork.MessageRepository.GetMessageAsync(id);

        if (message.Sender.UserName != User.GetUsername() && message.Recipient.UserName != User.GetUsername())
            return Unauthorized();

        if (message.Sender.UserName == User.GetUsername())
            message.SenderDeleted = true;

        if (message.Recipient.UserName == User.GetUsername())
            message.RecipientDeleted = true;

        if (message is { SenderDeleted: true, RecipientDeleted: true })
            unitOfWork.MessageRepository.DeleteMessage(message);

        if (await unitOfWork.MessageRepository.SaveAllAsync())
            return Ok();

        return BadRequest("Problem deleting the message");
    }
}