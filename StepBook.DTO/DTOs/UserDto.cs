namespace StepBook.DTO.DTOs;

/// <summary>
/// The user data transfer object
/// </summary>
public class UserDto
{
    /// <summary>
    /// The username
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// The token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// The refresh token
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The photo URL
    /// </summary>
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// The known as
    /// </summary>
    public required string KnownAs { get; set; }

    /// <summary>
    /// The gender
    /// </summary>
    public required string Gender { get; set; }
}