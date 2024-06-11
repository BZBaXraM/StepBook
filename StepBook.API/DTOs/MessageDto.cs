namespace StepBook.API.DTOs;

/// <summary>
/// Represents a message data transfer object.
/// </summary>
public class MessageDto
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
    /// The photo URL of the sender.
    /// </summary>
    public string SenderPhotoUrl { get; set; } = default!;

    /// <summary>
    /// The unique identifier of the recipient.
    /// </summary>
    public int RecipientId { get; set; }

    /// <summary>
    /// The username of the recipient.
    /// </summary>
    public string RecipientPhotoUrl { get; set; } = default!;

    /// <summary>
    /// The recipient of the message.
    /// </summary>
    public User Recipient { get; set; } = default!;

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
    public DateTime MessageSent { get; set; }
}