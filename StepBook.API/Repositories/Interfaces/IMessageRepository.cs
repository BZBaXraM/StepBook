namespace StepBook.API.Repositories.Interfaces;

/// <summary>
/// Interface for message repository.
/// </summary>
public interface IMessageRepository
{
    /// <summary>
    /// Add message to database.
    /// </summary>
    /// <param name="message"></param>
    Task AddMessageAsync(Message message);

    /// <summary>
    /// Delete message from database.
    /// </summary>
    /// <param name="message"></param>
    void DeleteMessage(Message message);

    /// <summary>
    /// Get message by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Message?> GetMessageAsync(int id);

    /// <summary>
    /// Get messages for user.
    /// </summary>
    /// <param name="messageParams"></param>
    /// <returns></returns>
    Task<PageList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);

    /// <summary>
    /// Get message thread.
    /// </summary>
    /// <param name="currentUsername"></param>
    /// <param name="recipientUsername"></param>
    /// <returns></returns>
    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsername, string recipientUsername);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    void AddGroup(Group group);

    /// <summary>
    /// Remove connection.
    /// </summary>
    /// <param name="connection"></param>
    void RemoveConnection(Connection connection);

    /// <summary>
    /// Get connection by id.
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    Task<Connection?> GetConnectionAsync(string connectionId);

    /// <summary>
    /// Get message group.
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    Task<Group?> GetMessageGroupAsync(string groupName);

    /// <summary>
    /// Get group for connection.
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    Task<Group?> GetGroupForConnectionAsync(string connectionId);
}