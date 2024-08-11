using Microsoft.AspNetCore.SignalR;
using StepBook.API.Contracts.Interfaces;

namespace StepBook.API.SignalR;

/// <inheritdoc />
public class MessageHub(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<PresenceHub> presenceHub) : Hub
{
    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        if (Context.User is null || string.IsNullOrEmpty(httpContext?.Request.Query["username"]))
            throw new HubException("User is null. Cannot get current user claim.");

        var groupName = GetGroupName(Context.User?.GetUsername()!, httpContext.Request.Query["username"]);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await AddGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", AddGroup(groupName));


        var messages = await unitOfWork.MessageRepository.GetMessageThreadAsync(Context.User!.GetUsername()!,
            httpContext.Request.Query["username"]!);

        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
        await base.OnDisconnectedAsync(exception);
    }

    /// <inheritdoc cref="createMessageDto" />
    public async Task SendMessage(CreateMessageRequestDto createMessageDto)
    {
        var username = Context.User?.GetUsername() ?? throw new Exception("could not get user");

        if (username == createMessageDto.RecipientUsername.ToLower())
            throw new HubException("You cannot message yourself");

        var sender = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var recipient = await unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

        if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null)
            throw new HubException("Cannot send message at this time");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };

        var groupName = GetGroupName(sender.UserName, recipient.UserName);
        var group = await unitOfWork.MessageRepository.GetMessageGroupAsync(groupName);

        if (group != null && group.Connections.Any(x => x.Username == recipient.UserName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
            if (connections != null && connections.Count != 0)
            {
                await presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, firstName = sender.FirstName });
            }
        }

        unitOfWork.MessageRepository.AddMessage(message);

        if (await unitOfWork.Complete())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
        }
    }


    private async Task AddGroup(string groupName)
    {
        var username = Context.User?.GetUsername() ?? throw new Exception("Cannot get username");
        var group = await unitOfWork.MessageRepository.GetMessageGroupAsync(groupName);
        var connection = new Connection { ConnectionId = Context.ConnectionId, Username = username };

        if (group == null)
        {
            group = new Group { Name = groupName };
            unitOfWork.MessageRepository.AddGroup(group);
        }

        group.Connections.Add(connection);

        if (await unitOfWork.Complete()) return;

        throw new HubException("Failed to join group");
    }

    private async Task RemoveFromGroup()
    {
        var group = await unitOfWork.MessageRepository.GetGroupForConnectionAsync(Context.ConnectionId);
        var connection = group?.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        if (connection != null && group != null)
        {
            unitOfWork.MessageRepository.RemoveConnection(connection);
            if (await unitOfWork.Complete()) return;
        }

        throw new Exception("Failed to remove from group");
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        var group = await unitOfWork.MessageRepository.GetGroupForConnectionAsync(Context.ConnectionId);
        var connection = group?.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        if (connection != null && group != null)
        {
            unitOfWork.MessageRepository.RemoveConnection(connection);
            if (await unitOfWork.Complete()) return group;
        }

        throw new Exception("Failed to remove from group");
    }

    private string GetGroupName(string? caller, string? other)
    {
        var compare = string.CompareOrdinal(caller, other) < 0;
        return compare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}