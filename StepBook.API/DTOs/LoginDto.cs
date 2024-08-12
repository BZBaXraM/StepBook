namespace StepBook.API.DTOs;

/// <summary>
/// DTO for user login.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// The email
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The username
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    /// The password
    /// </summary>
    public required string Password { get; init; } = null!;
}