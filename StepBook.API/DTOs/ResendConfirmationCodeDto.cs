namespace StepBook.API.DTOs;

/// <summary>
/// The DTO for resending a confirmation code
/// </summary>
public class ResendConfirmationCodeDto
{
    /// <summary>
    /// The email of the user
    /// </summary>
    public required string Email { get; set; }
}