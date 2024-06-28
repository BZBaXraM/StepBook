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
    /// <summary>
    /// Generate an email confirmation token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateEmailConfirmationToken(User user);
    
    /// <summary>
    /// Validate an email confirmation token
    /// </summary>
    /// <param name="user"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    bool ValidateEmailConfirmationToken(User user, string token);
}