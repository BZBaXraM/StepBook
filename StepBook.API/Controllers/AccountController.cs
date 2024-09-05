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
    IBlackListService blackListService,
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
        var user = mapper.Map<User>(dto);

        using var hmac = new HMACSHA512();

        user.UserName = dto.Username;
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        user.PasswordSalt = hmac.Key;
        user.EmailConfirmationToken = jwtService.GenerateEmailConfirmationToken(user);
        user.RefreshToken = jwtService.GenerateRefreshToken();

        await context.Users.AddAsync(user);
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
            .FirstOrDefaultAsync(x => x.UserName == dto.UsernameOrEmail || x.Email == dto.UsernameOrEmail);

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

        var refreshToken = jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        await context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user),
            RefreshToken = user.RefreshToken,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
        };
    }

    /// <summary>
    /// Refresh the token of a user
    /// </summary>
    /// <param name="tokenDto"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshTokenAsync([FromBody] RefreshTokenDto tokenDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.RefreshToken == tokenDto.RefreshToken);
        if (user == null)
        {
            return BadRequest("Invalid token");
        }

        var token = jwtService.GenerateSecurityToken(user);

        return new TokenDto
        {
            Token = token,
            RefreshToken = jwtService.GenerateRefreshToken(),
            RefreshTokenExpireTime = user.RefreshTokenExpireTime
        };
    }

    /// <summary>
    /// Logout a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync([FromBody] TokenDto dto)
    {
        var token = dto.Token;

        blackListService.AddTokenToBlackList(token);

        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
        if (user != null)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpireTime = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        return Ok("Logged out successfully");
    }


    /// <summary>
    /// Signin a user with Google
    /// </summary>
    /// <returns></returns>
    [HttpGet("signin-google")]
    public async Task<IActionResult> GoogleSignIn()
    {
        var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (response.Principal == null) return BadRequest();

        var email = response.Principal.FindFirstValue(ClaimTypes.Email);

        var user = await context.Users.Include(u => u.Photos)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null) return BadRequest();


        return Ok(new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
        });
    }

    /// <summary>
    /// Login with Google
    /// </summary>
    /// <returns></returns>
    [HttpGet("login-google")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleSignIn"),
            Items = { { "scheme", GoogleDefaults.AuthenticationScheme } }
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
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

    /// <summary>
    /// Change the password of a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("change-password")]
    public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

        if (user == null)
        {
            return NotFound("User not found");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.CurrentPassword));

        if (!computedHash.SequenceEqual(user.PasswordHash))
        {
            return Unauthorized("Invalid password");
        }

        if (dto.NewPassword != dto.ConfirmNewPassword)
        {
            return BadRequest("Passwords do not match");
        }

        using var newHmac = new HMACSHA512();
        user.PasswordHash = newHmac.ComputeHash(Encoding.UTF8.GetBytes(dto.NewPassword));
        user.PasswordSalt = newHmac.Key;

        await context.SaveChangesAsync();

        return Ok("Password changed successfully");
    }

    /// <summary>
    ///     Forget the password of a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("forget-password")]
    public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgetUserPasswordRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var resetCode = GenerateRandomCode();
        user.RandomCode = resetCode;
        await context.SaveChangesAsync();

        const string resetLink = "http://localhost:4200/reset-password";
        await emailService.SendEmailAsync(user.Email, "Reset your password",
            $"Your password reset code is: {resetCode}. You can also reset your password by clicking <a href='{resetLink}'>here</a>.");

        return Ok("Password reset code sent to your email");
    }

    /// <summary>
    /// Reset the password of a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }

        if (user.RandomCode != dto.Code)
        {
            return BadRequest("Invalid reset code");
        }

        using var hmac = new HMACSHA512();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.NewPassword));
        user.PasswordSalt = hmac.Key;
        user.RandomCode = null;

        await context.SaveChangesAsync();
        return Ok("Password reset successfully");
    }

    /// <summary>
    /// Delete the account of a user
    /// </summary>
    /// <returns></returns>
    [HttpDelete("delete-account")]
    [Authorize]
    public async Task<ActionResult> DeleteAccountAsync()
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
        if (user == null)
        {
            return NotFound("User not found");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return Ok("Account deleted successfully");
    }

    private string GenerateRandomCode(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new();

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}