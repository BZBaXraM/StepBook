namespace StepBook.API.DTOs;

/// <summary>
/// Represents the data transfer object for resending the confirmation code.
/// </summary>
public class ResendConfirmationCodeDto
{
    /// <summary>
    /// The email of the user.
    /// </summary>
    public required string Email { get; set; }
}