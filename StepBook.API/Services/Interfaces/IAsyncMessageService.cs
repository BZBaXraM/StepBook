namespace StepBook.API.Services.Interfaces;

public interface IAsyncMessageService
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessageAsync(int messageId);
    Task<PageList<MessageDto>> GetMessageForUserAsync();
    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int currentUserId, int recipientId);
    Task<bool> SaveAllAsync();
}