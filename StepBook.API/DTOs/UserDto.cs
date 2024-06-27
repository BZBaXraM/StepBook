namespace StepBook.API.DTOs;

/// <summary>
/// The user data transfer object
/// </summary>
public class UserDto
{
    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// The token
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// The photo URL
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// The known as
    /// </summary>
    public string KnownAs { get; set; } = null!;

    /// <summary>
    /// The refresh token
    /// </summary>
    public string? RefreshToken { get; set; }
}