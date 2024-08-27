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
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessageAsync(CreateMessageRequestDto dto)
    {
        var username = User.GetUsername();

        if (username == dto.RecipientUsername.ToLower())
            return BadRequest("You cannot message yourself");

        var sender = await unitOfWork.UserRepository.GetUserByUsernameAsync(username!);
        var recipient = await unitOfWork.UserRepository.GetUserByUsernameAsync(dto.RecipientUsername);

        if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null)
            return BadRequest("Cannot send message at this time");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = dto.Content
        };

        await unitOfWork.MessageRepository.AddMessageAsync(message);

        if (await unitOfWork.Complete())
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

        var messages = await unitOfWork.MessageRepository.GetMessagesForUserAsync(messageParams);

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
    public async Task<ActionResult> DeleteMessageAsync(int id)
    {
        var username = User.GetUsername();

        var message = await unitOfWork.MessageRepository.GetMessageAsync(id);

        if (message == null) return BadRequest("Cannot delete this message");

        if (message.SenderUsername != username && message.RecipientUsername != username)
            return Forbid();

        if (message.SenderUsername == username) message.SenderDeleted = true;
        if (message.RecipientUsername == username) message.RecipientDeleted = true;

        if (message is { SenderDeleted: true, RecipientDeleted: true })
        {
            unitOfWork.MessageRepository.DeleteMessage(message);
        }

        if (await unitOfWork.Complete()) return Ok();

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
        var count = await unitOfWork.MessageRepository.CountOfNewMessagesAsync(username!);
        return Ok(count);
    }
}