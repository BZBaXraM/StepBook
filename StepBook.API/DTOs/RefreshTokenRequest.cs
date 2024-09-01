namespace StepBook.API.DTOs;

/// <summary>
/// Refresh token request DTO
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// The refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

}