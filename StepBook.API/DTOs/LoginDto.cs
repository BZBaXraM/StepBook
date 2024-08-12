namespace StepBook.API.DTOs;

/// <summary>
/// DTO for user login.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// The email
    /// </summary>
    public required string Email { get; init; } = null!;

    /// <summary>
    /// The username
    /// </summary>
    public required string Username { get; init; } = null!;

    /// <summary>
    /// The password
    /// </summary>
    public required string Password { get; init; } = null!;
}