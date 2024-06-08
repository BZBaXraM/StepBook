namespace StepBook.API.Services.Classes;

/// <summary>
/// The JWT service
/// </summary>
/// <param name="config"></param>
public class JwtService(JwtConfig config) : IJwtService
{
    /// <summary>
    /// The JWT service configuration
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GenerateSecurityToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            }),
            Expires = DateTime.UtcNow.AddHours(config.Expiration),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}