namespace StepBook.DTO.DTOs;

/// <summary>
/// DTO for user login.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// The username or email
    /// </summary>
    public required string UsernameOrEmail { get; init; }

    /// <summary>
    /// The password
    /// </summary>
    public required string Password { get; init; } 
}