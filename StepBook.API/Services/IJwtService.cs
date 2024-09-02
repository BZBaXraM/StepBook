namespace StepBook.API.Services;

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
    Task<string> GenerateSecurityTokenAsync(User user);

    /// <summary>
    /// Generate a refresh token
    /// </summary>
    /// <returns></returns>
    Task<string> GenerateRefreshTokenAsync();

    /// <summary>
    /// Generate an email confirmation token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<string> GenerateEmailConfirmationTokenAsync(User user);

    /// <summary>
    /// Validate an email confirmation token
    /// </summary>
    /// <param name="user"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    bool ValidateEmailConfirmationToken(User user, string token);

    Task<string> GenerateForgetPasswordTokenAsync(User user);
    bool ValidateForgetPasswordToken(User user, string token);
    
    public ClaimsPrincipal GetPrincipalFromToken(string token);
}