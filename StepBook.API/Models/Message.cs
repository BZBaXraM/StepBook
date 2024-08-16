namespace StepBook.API.Models;

/// <summary>
/// Represents a message in the application.
/// </summary>
public class Message
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sender username
    /// </summary>
    public required string SenderUsername { get; set; }

    /// <summary>
    /// Recipient username
    /// </summary>
    public required string RecipientUsername { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Date read
    /// </summary>
    public DateTime? DateRead { get; set; }

    /// <summary>
    /// Message sent
    /// </summary>
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Sender deleted
    /// </summary>
    public bool SenderDeleted { get; set; }

    /// <summary>
    /// Recipient deleted
    /// </summary>
    public bool RecipientDeleted { get; set; }

    /// <summary>
    /// Sender id
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// Sender
    /// </summary>
    public User Sender { get; set; } = null!;

    /// <summary>
    /// Recipient id
    /// </summary>
    public int RecipientId { get; set; }

    /// <summary>
    /// Recipient
    /// </summary>
    public User Recipient { get; set; } = null!;
}