namespace StepBook.API.Repositories.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessageAsync(int messageId, CancellationToken cancellationToken = default);

    Task<PageList<MessageDto>> GetMessageForUserAsync(MessageParams messageParamsdefault);

    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername,
        CancellationToken cancellationToken = default);

    Task<bool> SaveAllAsync(CancellationToken cancellationToken = default);
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection> GetConnectionAsync(string connectionId, CancellationToken cancellationToken = default);
    Task<Group> GetMessageGroupAsync(string groupName, CancellationToken cancellationToken = default);
    Task<Group> GetGroupForConnectionAsync(string connectionId, CancellationToken cancellationToken = default);
}