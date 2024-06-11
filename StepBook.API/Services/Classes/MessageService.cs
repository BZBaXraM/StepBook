namespace StepBook.API.Services.Classes;

public class MessageService(StepContext context) : IAsyncMessageService
{
    public void AddMessage(Message message)
        => context.Messages.Add(message);


    public void DeleteMessage(Message message)
        => context.Messages.Remove(message);

    public async Task<Message> GetMessageAsync(int messageId)
        => (await context.Messages.FirstOrDefaultAsync(m => m.Id == messageId))!;

    public async Task<PageList<MessageDto>> GetMessageForUserAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(int currentUserId, int recipientId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
        => await context.SaveChangesAsync() > 0;
}