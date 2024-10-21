using System.Security.Claims;

namespace StepBook.API.Gateway.Services;

/// <summary>
/// The JWT service
/// </summary>
public interface IJwtService
{
    ClaimsPrincipal GetPrincipalFromToken(string token);
}