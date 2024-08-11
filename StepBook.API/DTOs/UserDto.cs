namespace StepBook.API.DTOs;

/// <summary>
/// The user data transfer object
/// </summary>
public class UserDto
{
    /// <summary>
    /// The username
    /// </summary>
    public required string Username { get; set; } = null!;

    /// <summary>
    /// The token
    /// </summary>
    public required string Token { get; set; } = null!;

    /// <summary>
    /// The photo URL
    /// </summary>
    public required string? PhotoUrl { get; set; }

    /// <summary>
    /// The first name
    /// </summary>
    public required string FirstName { get; set; } = null!;

    /// <summary>
    /// The gender
    /// </summary>
    public required string Gender { get; set; } = null!;

    /// <summary>
    /// The refresh token
    /// </summary>
    public required string? RefreshToken { get; set; }
}