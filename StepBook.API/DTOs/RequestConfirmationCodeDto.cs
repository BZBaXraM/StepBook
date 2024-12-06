namespace StepBook.API.DTOs;

/// <summary>
/// DTO for request confirmation code
/// </summary>
public class RequestConfirmationCodeDto
{
    /// <summary>
    /// Email of the user
    /// </summary>
    public required string Email { get; set; }
}