using StepBook.Domain.Entities;

namespace Messages.API.Models;

public class Message
{
    public int Id { get; set; }
    public required string SenderUsername { get; set; }
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }

    private DateTime? _dateRead;

    public DateTime? DateRead
    {
        get => _dateRead;
        set => _dateRead = value?.ToUniversalTime();
    }

    private DateTime _messageSent = DateTime.UtcNow;

    public DateTime MessageSent
    {
        get => _messageSent;
        set => _messageSent = value.ToUniversalTime();
    }

    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
    public int SenderId { get; set; }
    public UserBasic Sender { get; set; } = null!;
    public int RecipientId { get; set; }
    public UserBasic Recipient { get; set; } = null!;
    public string? FileUrl { get; set; }
}