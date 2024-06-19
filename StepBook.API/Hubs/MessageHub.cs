namespace StepBook.API.Hubs;

public class MessageHub(
    IAsyncMessageService messageService,
    IAsyncUserService userService,
    IMapper mapper) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request.Query["user"].ToString();
        var groupName = GetGroupName(Context.User!.GetUsername()!, otherUser!);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var messages = await messageService.GetMessageThreadAsync(Context.User!.GetUsername()!, otherUser!);

        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessageRequestDto dto)
    {
        var username = Context.User!.GetUsername()!;

        if (username == dto.RecipientUsername)
            throw new HubException("You cannot send messages to yourself");

        var sender = await userService.GetUserByUserNameAsync(username);
        var recipient = await userService.GetUserByUserNameAsync(dto.RecipientUsername);

        if (recipient is null) throw new HubException("Not found user");

        var message = new Message
        {
            SenderId = sender.Id,
            RecipientId = recipient.Id,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = dto.Content
        };

        messageService.AddMessage(message);

        if (await messageService.SaveAllAsync())
        {
            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
        }
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}