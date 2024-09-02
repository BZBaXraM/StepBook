namespace StepBook.API.DTOs;

/// <summary>
/// Refresh token DTO
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } 

    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; }
}