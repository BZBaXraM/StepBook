namespace StepBook.API.DTOs;

/// <summary>
/// Data transfer object for message
/// </summary>
public class MessageDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sender id
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// Sender username
    /// </summary>
    public required string SenderUsername { get; set; }

    /// <summary>
    /// Sender photo url
    /// </summary>
    public required string SenderPhotoUrl { get; set; }

    /// <summary>
    /// Recipient id
    /// </summary>
    public Guid RecipientId { get; set; }

    /// <summary>
    /// Recipient username
    /// </summary>
    public required string RecipientUsername { get; set; }

    /// <summary>
    /// Recipient photo url
    /// </summary>
    public required string RecipientPhotoUrl { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// File url
    /// </summary>
    public string? FileUrl { get; set; }

    /// <summary>
    /// Date read
    /// </summary>
    public DateTime? DateRead { get; set; }

    /// <summary>
    /// Message sent
    /// </summary>
    public DateTime MessageSent { get; set; }
}