namespace StepBook.DTO.DTOs;

/// <summary>
/// Token DTO
/// </summary>
public class TokenDto
{
    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = default!;
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = default!;
    /// <summary>
    /// Token expire time
    /// </summary>
    public DateTime RefreshTokenExpireTime { get; set; }
}