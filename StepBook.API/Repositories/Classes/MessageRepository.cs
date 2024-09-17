using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Repositories.Classes;

/// <summary>
/// Message service
/// </summary>
/// <param name="context"></param>
/// <param name="mapper"></param>
public class MessageRepository(StepContext context, IMapper mapper) : IMessageRepository
{
    /// <summary>
    /// Add group to the context.
    /// </summary>
    /// <param name="group"></param>
    public async Task AddGroupAsync(Group group)
    {
        await context.Groups.AddAsync(group);
    }

    /// <summary>
    /// Add message to the context.
    /// </summary>
    /// <param name="message"></param>
    public async Task AddMessageAsync(Message message)
    {
        await context.Messages.AddAsync(message);
    }

    /// <summary>
    /// Delete message from the context.
    /// </summary>
    /// <param name="message"></param>
    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    /// <summary>
    /// Get message by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Message?> GetMessageAsync(int id)
    {
        return await context.Messages.FindAsync(id);
    }

    /// <summary>
    /// Get connection by id.
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public async Task<Connection?> GetConnectionAsync(string connectionId)
        => await context.Connections.FindAsync(connectionId);

    /// <summary>
    /// Get group for connection.
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public async Task<Group?> GetGroupForConnectionAsync(string connectionId)
    {
        return await context.Groups
            .Include(x => x.Connections)
            .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get message by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Message?> GetMessage(int id)
        => await context.Messages.FindAsync(id);

    /// <summary>
    /// Get message group by name.
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public async Task<Group?> GetMessageGroupAsync(string groupName)
    {
        return await context.Groups
            .Include(x => x.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName);
    }

    /// <summary>
    /// Get messages for user.
    /// </summary>
    /// <param name="messageParams"></param>
    /// <returns></returns>
    public async Task<PageList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
    {
        var query = context.Messages
            .OrderByDescending(x => x.MessageSent)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(x => x.Recipient.UserName == messageParams.Username
                                        && x.RecipientDeleted == false),
            "Outbox" => query.Where(x => x.Sender.UserName == messageParams.Username
                                         && x.SenderDeleted == false),
            _ => query.Where(x => x.Recipient.UserName == messageParams.Username && x.DateRead == null
                && x.RecipientDeleted == false)
        };

        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

        return await PageList<MessageDto>.CreateAsync(messages, messageParams.PageNumber,
            messageParams.PageSize);
    }

    public async Task<List<Message>> GetPendingMessagesAsync()
    {
        var query = context.Messages
            .Where(x => x.DateRead == null)
            .OrderBy(x => x.MessageSent)
            .AsQueryable();

        return await query.ToListAsync();
    }

    /// <summary>
    /// Get message thread.
    /// </summary>
    /// <param name="currentUsername"></param>
    /// <param name="recipientUsername"></param>
    /// <returns></returns>
    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername)
    {
        var query = context.Messages
            .Where(x =>
                x.RecipientUsername == currentUsername
                && x.RecipientDeleted == false
                && x.SenderUsername == recipientUsername ||
                x.SenderUsername == currentUsername
                && x.SenderDeleted == false
                && x.RecipientUsername == recipientUsername
            )
            .OrderBy(x => x.MessageSent)
            .AsQueryable();

        var unreadMessages = query.Where(x => x.DateRead == null &&
                                              x.RecipientUsername == currentUsername).ToList();

        if (unreadMessages.Count != 0)
        {
            unreadMessages.ForEach(x => x.DateRead = DateTime.UtcNow);
        }

        return await query.ProjectTo<MessageDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    /// <summary>
    /// Count of new messages.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<int> CountOfNewMessagesAsync(string username)
    {
        return await context.Messages
            .CountAsync(x => x.Recipient.UserName == username && x.DateRead == null);
    }

    public async Task UpdateMessageAsync(Message message)
    {
        context.Messages.Update(message);
        await context.SaveChangesAsync();
    }


    /// <summary>
    /// Remove connection from the context.
    /// </summary>
    /// <param name="connection"></param>
    public void RemoveConnection(Connection connection)
    {
        context.Connections.Remove(connection);
    }
}