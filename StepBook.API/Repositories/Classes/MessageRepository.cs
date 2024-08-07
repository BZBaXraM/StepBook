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
    /// Add message to the database
    /// </summary>
    /// <param name="message"></param>
    public void AddMessage(Message message)
        => context.Messages.Add(message);


    /// <summary>
    /// Delete message from the database
    /// </summary>
    /// <param name="message"></param>
    public void DeleteMessage(Message message)
        => context.Messages.Remove(message);

    /// <summary>
    /// Get message by id
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    public async Task<Message> GetMessageAsync(int messageId, CancellationToken cancellationToken = default)
        => (await context.Messages.FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken: cancellationToken))!;

    /// <summary>
    ///  Get messages for user
    /// </summary>
    /// <param name="messageParams"></param>
    /// <returns></returns>
    public async Task<PageList<MessageDto>> GetMessageForUserAsync(MessageParams messageParams)
    {
        var query = context.Messages
            .OrderByDescending(m => m.MessageSent)
            .AsNoTracking()
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false),
            "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username && u.SenderDeleted == false),
            _ => query.Where(u =>
                u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
        };

        return await PageList<MessageDto>.CreateAsync(query.ProjectTo<MessageDto>(mapper.ConfigurationProvider),
            PaginationParams.PageNumber, messageParams.PageSize);
    }

    /// <summary>
    /// Get message thread
    /// </summary>
    /// <param name="currentUsername"></param>
    /// <param name="recipientUsername"></param>
    /// <returns></returns>
    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername,
        CancellationToken cancellationToken = default)
    {
        var messages = await context.Messages
            .Include(x => x.Sender).ThenInclude(x => x.Photos)
            .Include(x => x.Recipient).ThenInclude(x => x.Photos)
            .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                                                                && m.Sender.UserName == recipientUsername
                        || m.Recipient.UserName == recipientUsername && m.Sender.UserName == currentUsername
                                                                     && m.SenderDeleted == false)
            .OrderBy(m => m.MessageSent)
            .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        var unreadMessages = messages.Where(m =>
                m.DateRead is null
                && m.SenderUsername == currentUsername)
            .ToList();

        if (unreadMessages.Count == 0)
        {
            return mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        foreach (var item in unreadMessages)
        {
            item.DateRead = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    /// <summary>
    /// Save all changes
    /// </summary>
    /// <returns></returns>
    public async Task<bool> SaveAllAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken) > 0;

    /// <summary>
    /// Set message as read
    /// </summary>
    /// <param name="group"></param>
    public void AddGroup(Group group)
    {
        context.Groups.Add(group);
    }

    /// <summary>
    /// Remove group
    /// </summary>
    /// <param name="connection"></param>
    public void RemoveConnection(Connection connection)
    {
        context.Connections.Remove(connection);
    }

    /// <summary>
    /// Get connection by id
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public async Task<Connection> GetConnectionAsync(string connectionId, CancellationToken cancellationToken = default)
        => (await context.Connections.FindAsync([connectionId], cancellationToken: cancellationToken))!;

    /// <summary>
    /// Get connections for user
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public async Task<Group> GetMessageGroupAsync(string groupName, CancellationToken cancellationToken = default)
    {
        return (await context.Groups
            .Include(x => x.Connections)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == groupName, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// Get group for connection
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    public async Task<Group> GetGroupForConnectionAsync(string connectionId,
        CancellationToken cancellationToken = default)
    {
        return (await context.Groups
            .Include(x => x.Connections)
            .AsNoTracking()
            .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken))!;
    }
}