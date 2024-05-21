using System.Security.Claims;

namespace StepBook.API.Services.Interfaces;

public interface IJwtService
{
    string GenerateSecurityToken(
        string id,
        string email,
        IEnumerable<string> roles,
        IEnumerable<Claim> userClaims);
}