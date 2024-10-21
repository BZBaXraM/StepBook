using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messages.API.Shared.Configs;
using Microsoft.IdentityModel.Tokens;

namespace Messages.API.Services;

/// <summary>
/// The JWT service
/// </summary>
/// <param name="config"></param>
public class JwtService(JwtConfig config)
    : IJwtService
{
    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.Secret);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null!;
        }
    }
}