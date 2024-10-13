using System.Security.Claims;
using StepBook.Domain.Entities;

namespace AuthMiddleware.Jwt;

/// <summary>
/// The JWT service
/// </summary>
public interface IJwtService
{
    string GenerateSecurityToken(User user);

    string GenerateEmailConfirmationToken(User user);
    bool ValidateEmailConfirmationToken(User user, string token);
    string GenerateForgetPasswordToken(User user);
    bool ValidateForgetPasswordToken(User user, string token);
    ClaimsPrincipal GetPrincipalFromToken(string token);
    string GenerateRefreshToken();
    string GenerateRefreshTokenForEmail(User user);
}