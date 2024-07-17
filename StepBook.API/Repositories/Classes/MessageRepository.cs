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
    public async Task<Message> GetMessageAsync(int messageId)
        => (await context.Messages.FirstOrDefaultAsync(m => m.Id == messageId))!;

    public async Task<PageList<MessageDto>> GetMessageForUserAsync(MessageParams messageParams)
    {
        var query = context.Messages
            .OrderByDescending(m => m.MessageSent)
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

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername)
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
            .ToListAsync();

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

        await context.SaveChangesAsync();
        return mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<bool> SaveAllAsync()
        => await context.SaveChangesAsync() > 0;

    public void AddGroup(Group group)
    {
        context.Groups.Add(group);
    }

    public void RemoveConnection(Connection connection)
    {
        context.Connections.Remove(connection);
    }

    public async Task<Connection> GetConnectionAsync(string connectionId)
        => (await context.Connections.FindAsync(connectionId))!;

    public async Task<Group> GetMessageGroupAsync(string groupName)
    {
        return (await context.Groups
            .Include(x => x.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName))!;
    }

    public async Task<Group> GetGroupForConnectionAsynv(string connectionId)
    {
        return (await context.Groups
            .Include(x => x.Connections)
            .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync())!;
    }
}