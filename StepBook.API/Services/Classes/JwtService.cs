using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StepBook.API.Data.Entities;
using StepBook.API.Services.Interfaces;

namespace StepBook.API.Services.Classes;

public class JwtService(JwtConfig config) : IJwtService
{
    public string GenerateSecurityToken(string id, string email, IEnumerable<string> roles,
        IEnumerable<Claim> userClaims)
    {
        var claims = new[]
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, string.Join(",", roles)),
            new Claim("userId", id)
        }.Concat(userClaims);

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(config.Secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: config.Issuer,
            audience: config.Audience, expires: DateTime.UtcNow.AddMinutes(config.Expiration), claims: claims,
            signingCredentials: signingCredentials);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }
}