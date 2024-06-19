namespace StepBook.API.Hubs;

public class MessageHub(
    IAsyncMessageService messageService,
    IAsyncUserService userService,
    IMapper mapper,
    IHubContext<PresenceHub> context,
    PresenceTracker tracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request.Query["user"].ToString();
        var groupName = GetGroupName(Context.User!.GetUsername()!, otherUser!);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await AddToGroupAsync(Context, groupName);

        var messages = await messageService.GetMessageThreadAsync(Context.User!.GetUsername()!, otherUser!);

        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await RemoveFromGroupAsync(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(CreateMessageRequestDto dto)
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

        var groupName = GetGroupName(sender.UserName, recipient.UserName);

        var group = await messageService.GetMessageGroupAsync(groupName);

        if (group.Connections.Any(x => x.Username == recipient.UserName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await tracker.GetConnectionsForUser(recipient.UserName);
            if (connections is not null)
            {
                await context.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, knownAs = sender.KnownAs });
            }
        }


        messageService.AddMessage(message);

        if (await messageService.SaveAllAsync())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
        }
    }

    private async Task AddToGroupAsync(HubCallerContext context, string groupName)
    {
        var group = await messageService.GetMessageGroupAsync(groupName);
        var connection = new Connection
        {
            ConnectionId = context.ConnectionId,
            Username = context.User!.GetUsername()!
        };

        if (group is null)
        {
            group = new Group(groupName);
            messageService.AddGroup(group);
        }

        group.Connections.Add(connection);

        await messageService.SaveAllAsync();
    }

    private async Task RemoveFromGroupAsync(string connectionId)
    {
        var connection = await messageService.GetConnectionAsync(connectionId);
        messageService.RemoveConnection(connection);
        await messageService.SaveAllAsync();
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}