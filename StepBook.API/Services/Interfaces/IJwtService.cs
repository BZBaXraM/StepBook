namespace StepBook.API.Services.Interfaces;

/// <summary>
/// The JWT service
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generate a security token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateSecurityToken(User user);

    /// <summary>
    /// Generate a refresh token
    /// </summary>
    /// <returns></returns>
    string GenerateRefreshToken();
    string GenerateEmailConfirmationToken(User user);
}