namespace StepBook.API.DTOs;

/// <summary>
/// DTO for user registration.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// The username
    /// </summary>
    public required string Username { get; init; } = null!;

    /// <summary>
    /// The password
    /// </summary>
    public required string Password { get; init; } = null!;
}