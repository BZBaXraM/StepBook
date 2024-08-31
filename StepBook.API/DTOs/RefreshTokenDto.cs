namespace StepBook.API.DTOs;

/// <summary>
/// Refresh token DTO
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = default!;
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}