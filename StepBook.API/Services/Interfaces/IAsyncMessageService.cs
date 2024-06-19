namespace StepBook.API.Services.Interfaces;

public interface IAsyncMessageService
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessageAsync(int messageId);
    Task<PageList<MessageDto>> GetMessageForUserAsync(MessageParams messageParams);
    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername);
    Task<bool> SaveAllAsync();
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection> GetConnectionAsync(string connectionId);
    Task<Group> GetMessageGroupAsync(string groupName);
}