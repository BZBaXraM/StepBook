using System.Security.Claims;

namespace Messages.API.Services;

/// <summary>
/// The JWT service
/// </summary>
public interface IJwtService
{
    ClaimsPrincipal GetPrincipalFromToken(string token);
}