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
    IAsyncMessageService messageService,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost("create")]
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
}