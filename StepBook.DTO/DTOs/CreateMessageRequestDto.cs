namespace StepBook.DTO.DTOs;

/// <summary>
/// Create message request data transfer object.
/// </summary>
public class CreateMessageRequestDto
{
    /// <summary>
    /// Recipient username.
    /// </summary>
    public required string RecipientUsername { get; set; }

    /// <summary>
    /// Message content.
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// File URL.
    /// </summary>
    public string? FileUrl { get; set; }
}