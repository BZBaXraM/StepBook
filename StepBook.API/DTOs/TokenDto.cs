namespace StepBook.API.DTOs;

/// <summary>
/// Token DTO
/// </summary>
public class TokenDto
{
    /// <summary>
    /// Access token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } 

    /// <summary>
    /// Refresh token expire time
    /// </summary>
    public DateTime RefreshTokenExpireTime { get; set; }
}