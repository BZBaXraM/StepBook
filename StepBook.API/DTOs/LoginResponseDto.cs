namespace StepBook.API.DTOs;

/// <summary>
/// Login response DTO
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Access token for the user
    /// </summary>
    public string AccessToken { get; set; } = null!;
    /// <summary>
    /// Refresh token for the user
    /// </summary>
    public string RefreshToken { get; set; } = null!;
}