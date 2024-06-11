namespace StepBook.API.DTOs;

/// <summary>
/// Data transfer object for message
/// </summary>
public class MessageDto
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Sender id
    /// </summary>
    public int SenderId { get; set; }
    public string SenderUsername { get; set; } = default!;
    public int RecipientId { get; set; }
    public string? RecipientPhotoUrl { get; set; }
    public string Content { get; set; } = default!;
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }
}