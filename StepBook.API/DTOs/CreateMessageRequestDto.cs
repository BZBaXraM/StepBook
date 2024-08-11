namespace StepBook.API.DTOs;

/// <summary>
/// Create message request data transfer object.
/// </summary>
public class CreateMessageRequestDto
{
    /// <summary>
    /// Recipient username.
    /// </summary>
    public required string RecipientUsername { get; set; } = null!;
    /// <summary>
    /// Message content.
    /// </summary>
    public required string Content { get; set; } = null!;
}