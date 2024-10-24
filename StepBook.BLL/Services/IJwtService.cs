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
    string GenerateSecurityToken(User user);

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

    /// <summary>
    /// Generate a forget password token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateForgetPasswordToken(User user);

    /// <summary>
    /// Validate a forget password token
    /// </summary>
    /// <param name="user"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    bool ValidateForgetPasswordToken(User user, string token);

    /// <summary>
    /// Get the principal from a token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    ClaimsPrincipal GetPrincipalFromToken(string token);

    /// <summary>
    /// Generate a refresh token
    /// </summary>
    /// <returns></returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Generate a refresh token for email
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateRefreshTokenForEmail(User user);
}