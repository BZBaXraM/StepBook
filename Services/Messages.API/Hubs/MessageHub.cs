using AutoMapper;
using BuildingBlocks.Extensions;
using Messages.API.Data;
using Messages.API.DTOs;
using Messages.API.Models;
using Messages.API.Repositories;
using Messages.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace Messages.API.Hubs;

public class MessageHub(
    MessageContext context,
    IMessageRepository messageRepository,
    UserService userService,
    IMapper mapper,
    ILogger<MessageHub> logger,
    IHubContext<PresenceHub> presenceHub) : Hub
{
    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request.Query["user"];

        if (Context.User == null || string.IsNullOrEmpty(otherUser))
            throw new Exception("Cannot join group");

        var groupName = GetGroupName(Context.User.GetUsername()!, otherUser);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages =
            await messageRepository.GetMessageThreadAsync(Context.User.GetUsername()!, otherUser!);

        if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Send message to user.
    /// </summary>
    /// <param name="dto"></param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="HubException"></exception>
    public async Task SendMessage(CreateMessageRequestDto dto)
    {
        var username = Context.User?.GetUsername() ?? throw new Exception("Cannot get username");

        if (username == dto.RecipientUsername.ToLower())
            throw new HubException("You cannot message yourself");

        var sender =
            await userService
                .GetUserByUsernameAsync(
                    username ??
                    throw new HubException("Sender not found"));

        var recipient = await userService.GetUserByUsernameAsync(dto.RecipientUsername);

        if (sender == null)
        {
            logger.LogError("User {Username} not found in the repository", username);
        }

        if (recipient == null)
        {
            logger.LogError("Recipient {RecipientUsername} not found in the repository", dto.RecipientUsername);
            throw new HubException("Recipient not found");
        }

        if (recipient == null || sender?.UserName == null || recipient.UserName == null)
            throw new HubException("Cannot send message at this time");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = dto.Content,
            FileUrl = dto.FileUrl
        };

        var groupName = GetGroupName(sender.UserName, recipient.UserName);
        var group = await messageRepository.GetMessageGroupAsync(groupName);

        if (group != null && group.Connections.Any(x => x.Username == recipient.UserName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
            if (connections?.Count != null)
            {
                await presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new { username = sender.UserName, knownAs = sender.KnownAs });
            }
        }

        await messageRepository.AddMessageAsync(message);

        if (await context.SaveChangesAsync() > 0)
        {
            await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
        }
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        var username = Context.User?.GetUsername() ?? throw new Exception("Cannot get username");
        var group = await messageRepository.GetMessageGroupAsync(groupName);
        var connection = new Connection { ConnectionId = Context.ConnectionId, Username = username };

        if (group == null)
        {
            group = new Group { Name = groupName };
            await messageRepository.AddGroupAsync(group);
        }

        group.Connections.Add(connection);

        if (await context.SaveChangesAsync() > 0) return group;

        throw new HubException("Failed to join group");
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        var group = await messageRepository.GetGroupForConnectionAsync(Context.ConnectionId);
        var connection = group?.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        if (connection != null && group != null)
        {
            messageRepository.RemoveConnection(connection);
            if (await context.SaveChangesAsync() > 0) return group;
        }

        throw new Exception("Failed to remove from group");
    }

    private static string GetGroupName(string caller, string? other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}