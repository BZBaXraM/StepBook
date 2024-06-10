namespace StepBook.API.Models;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; } = default!;
    public User Sender { get; set; }
    public int RecipientId { get; set; }
    public string RecipientUsername { get; set; } = default!;
    public User Recipient { get; set; }
    public string Content { get; set; } = default!;
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.Now;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
}