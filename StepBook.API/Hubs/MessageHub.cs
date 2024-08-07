using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Hubs;

public class MessageHub(
    IMessageRepository messageRepository,
    IUserRepository userRepository,
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
        var group = await AddToGroupAsync(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages = await messageRepository.GetMessageThreadAsync(Context.User!.GetUsername()!, otherUser!);

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var group = await RemoveFromGroupAsync();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageAsync(CreateMessageRequestDto dto)
    {
        var username = Context.User!.GetUsername()!;

        if (username == dto.RecipientUsername)
            throw new HubException("You cannot send messages to yourself");

        var sender = await userRepository.GetUserByUserNameAsync(username);
        var recipient = await userRepository.GetUserByUserNameAsync(dto.RecipientUsername);

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

        var group = await messageRepository.GetMessageGroupAsync(groupName);

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


        messageRepository.AddMessage(message);

        if (await messageRepository.SaveAllAsync())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
        }
    }

    private async Task<Group> AddToGroupAsync(string groupName)
    {
        var group = await messageRepository.GetMessageGroupAsync(groupName);
        var connection = new Connection(Context.ConnectionId, Context.User!.GetUsername()!);

        if (group is null)
        {
            group = new Group(groupName);
            messageRepository.AddGroup(group);
        }

        group.Connections.Add(connection);
        if (await messageRepository.SaveAllAsync()) return group;

        throw new HubException("Failed to add to group");
    }

    private async Task<Group> RemoveFromGroupAsync()
    {
        var group = await messageRepository.GetGroupForConnectionAsync(Context.ConnectionId);
        var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

        messageRepository.RemoveConnection(connection!);
        if (await messageRepository.SaveAllAsync()) return group;

        throw new HubException("Failed to remove from group");
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}