namespace StepBook.API.Controllers;

/// <summary>
/// Account controller
/// </summary>
/// <param name="context"></param>
/// <param name="jwtService"></param>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
public class AccountController(
    StepContext context,
    IJwtService jwtService,
    IEmailService emailService,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] RegisterDto dto)
    {
        if (await context.Users.AnyAsync(x => x.UserName == dto.Username || x.Email == dto.Email))
        {
            return BadRequest("Username or Email is already taken");
        }

        var user = mapper.Map<User>(dto);

        using var hmac = new HMACSHA512();

        user.UserName = dto.Username;
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        user.PasswordSalt = hmac.Key;
        user.EmailConfirmationToken = jwtService.GenerateEmailConfirmationToken(user);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var confirmLink = Url.Action("ConfirmEmail", "Account",
            new { token = user.EmailConfirmationToken, email = user.Email }, Request.Scheme);
        await emailService.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your email by clicking <a href='{confirmLink}'>here</a>.");

        return Ok("Registration successful. Please check your email for confirmation link.");
    }


    /// <summary>
    /// Login a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginAsync([FromBody] LoginDto dto)
    {
        var user = await context.Users
            .Include(u => u.Photos)
            .SingleOrDefaultAsync(x => x.UserName == dto.UsernameOrEmail || x.Email == dto.UsernameOrEmail);

        if (user == null)
        {
            return Unauthorized("Invalid username or email");
        }

        if (!user.IsEmailConfirmed)
        {
            return Unauthorized("Email not confirmed. Please check your email.");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
        {
            return Unauthorized("Invalid password");
        }

        user.RefreshToken = jwtService.GenerateRefreshToken();

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs!,
            RefreshToken = user.RefreshToken
        };
    }

    /// <summary>
    /// Refresh the token of a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<UserDto>> RefreshTokenAsync([FromBody] RefreshTokenDto dto)
    {
        var user = await context.Users.Include(user => user.Photos).SingleOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);

        if (user == null)
        {
            return Unauthorized("Invalid refresh token");
        }

        user.RefreshToken = jwtService.GenerateRefreshToken();

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs!,
            RefreshToken = user.RefreshToken
        };
    }

    /// <summary>
    /// Confirm the email of a user
    /// </summary>
    /// <param name="token"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("confirm-email")]
    public async Task<ActionResult> ConfirmEmailAsync([FromQuery] string token, string email)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var isValid = jwtService.ValidateEmailConfirmationToken(user, token);
        if (!isValid)
        {
            return BadRequest("Invalid token");
        }

        user.IsEmailConfirmed = true;
        await context.SaveChangesAsync();

        return Ok("Email confirmed successfully. You can now log in.");
    }
}