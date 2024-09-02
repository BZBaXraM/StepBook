using StepBook.API.Data.Configs;

namespace StepBook.API.Services;

/// <summary>
/// The JWT service
/// </summary>
/// <param name="config"></param>
public class JwtService(IConfiguration config) : IJwtService
{
    /// <summary>
    /// The JWT service configuration
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<string> GenerateSecurityTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.GetSection("JWT:Key").Value!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            ]),
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(config.GetSection("JWT:Expires").Value)),
            Issuer = config.GetSection("JWT:Issuer").Value, // Ensure this is set
            Audience = config.GetSection("JWT:Audience").Value, SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    public async Task<string> GenerateRefreshTokenAsync()
    {
        return await Task.FromResult(Guid.NewGuid().ToString());
    }

    /// <summary>
    /// Generate an email confirmation token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.GetSection("JWT:Key").Value!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(config.GetSection("JWT:Expires").Value)),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    /// <inheritdoc />
    public bool ValidateEmailConfirmationToken(User user, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.GetSection("JWT:Key").Value!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value == user.Email;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<string> GenerateForgetPasswordTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.GetSection("JWT:Key").Value!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires =
                DateTime.UtcNow.AddHours(Convert.ToDouble(config.GetSection("JWT:ForgetPasswordExpires").Value)),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    /// <inheritdoc />
    public bool ValidateForgetPasswordToken(User user, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config.GetSection("JWT:Key").Value!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value == user.Email;
        }
        catch
        {
            return false;
        }
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
}