using System.Security.Claims;

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
}