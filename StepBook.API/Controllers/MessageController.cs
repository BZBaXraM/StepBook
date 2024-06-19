namespace StepBook.API.Controllers;

/// <summary>
/// Message controller
/// </summary>
/// <param name="userService"></param>
/// <param name="messageService"></param>
/// <param name="mapper"></param>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessageController(
    IAsyncUserService userService,
    IMessageService messageService,
    IMapper mapper)
    : ControllerBase
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

        var sender = await userService.GetUserByUserNameAsync(username);
        var recipient = await userService.GetUserByUserNameAsync(messageCreateDto.RecipientUsername);

        if (recipient is null) return NotFound();

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = messageCreateDto.Content
        };

        messageService.AddMessage(message);

        if (await messageService.SaveAllAsync())
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

        var messages = await messageService.GetMessageForUserAsync(messageParams);

        Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

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
        var messages = await messageService.GetMessageThreadAsync(User.GetUsername()!, username);
        return Ok(mapper.Map<IEnumerable<MessageDto>>(messages));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var message = await messageService.GetMessageAsync(id);

        if (message.Sender.UserName != User.GetUsername() && message.Recipient.UserName != User.GetUsername())
            return Unauthorized();

        if (message.Sender.UserName == User.GetUsername())
            message.SenderDeleted = true;

        if (message.Recipient.UserName == User.GetUsername())
            message.RecipientDeleted = true;

        if (message is { SenderDeleted: true, RecipientDeleted: true })
            messageService.DeleteMessage(message);

        if (await messageService.SaveAllAsync())
            return Ok();

        return BadRequest("Problem deleting the message");
    }
}