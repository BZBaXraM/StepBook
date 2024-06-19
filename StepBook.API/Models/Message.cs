namespace StepBook.API.Models;

/// <summary>
/// Represents a message in the application.
/// </summary>
public class Message
{
    /// <summary>
    /// The unique identifier for the message.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique identifier of the sender.
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// The username of the sender.
    /// </summary>
    public string SenderUsername { get; set; } = default!;

    /// <summary>
    /// The sender of the message.
    /// </summary>
    public User Sender { get; set; } = default!;

    /// <summary>
    /// The unique identifier of the recipient.
    /// </summary>
    public int RecipientId { get; set; }

    /// <summary>
    /// The username of the recipient.
    /// </summary>
    public string RecipientUsername { get; set; } = default!;

    /// <summary>
    /// The recipient of the message.
    /// </summary>
    public User Recipient { get; set; }

    /// <summary>
    /// The content of the message.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// The date the message was read.
    /// </summary>
    public DateTime? DateRead { get; set; }

    /// <summary>
    /// The date the message was sent.
    /// </summary>
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The sender deleted the message.
    /// </summary>
    public bool SenderDeleted { get; set; }

    /// <summary>
    /// The recipient deleted the message.
    /// </summary>
    public bool RecipientDeleted { get; set; }
}