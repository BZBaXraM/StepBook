namespace StepBook.API.DTOs;

/// <summary>
/// Refresh token DTO
/// </summary>
public class TokenDto
{
    /// <summary>
    /// The token
    /// </summary>
    public string AccessToken { get; set; }  = string.Empty;

    /// <summary>
    /// The refresh token
    /// </summary>
    public string? RefreshToken { get; set; } = string.Empty;
}