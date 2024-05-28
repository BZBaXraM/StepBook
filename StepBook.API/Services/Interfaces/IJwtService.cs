using System.Security.Claims;

namespace StepBook.API.Services.Interfaces;

public interface IJwtService
{
    string GenerateSecurityToken(User user);
}