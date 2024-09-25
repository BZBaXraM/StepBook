using System.Security.Claims;
using StepBook.Domain.Entities;

namespace Account.API.Features.Account;

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

    string GenerateForgetPasswordToken(User user);
    bool ValidateForgetPasswordToken(User user, string token);
    
    ClaimsPrincipal GetPrincipalFromToken(string token);
    
    string GenerateRefreshToken();
    string GenerateRefreshTokenForEmail(User user);
}