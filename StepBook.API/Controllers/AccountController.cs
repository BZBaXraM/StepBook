using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Controllers;

/// <summary>
/// Account controller
/// </summary>
/// <param name="context"></param>
/// <param name="jwtRepository"></param>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
public class AccountController(
    StepContext context,
    IJwtRepository jwtRepository,
    IEmailRepository emailRepository,
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
        user.EmailConfirmationToken = jwtRepository.GenerateEmailConfirmationToken(user);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var confirmLink = Url.Action("ConfirmEmail", "Account",
            new { token = user.EmailConfirmationToken, email = user.Email }, Request.Scheme);
        await emailRepository.SendEmailAsync(user.Email, "Confirm your email",
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

        user.RefreshToken = jwtRepository.GenerateRefreshToken();

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtRepository.GenerateSecurityToken(user),
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
        var user = await context.Users.Include(user => user.Photos)
            .SingleOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);

        if (user == null)
        {
            return Unauthorized("Invalid refresh token");
        }

        user.RefreshToken = jwtRepository.GenerateRefreshToken();

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtRepository.GenerateSecurityToken(user),
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

        var isValid = jwtRepository.ValidateEmailConfirmationToken(user, token);
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
        await emailRepository.SendEmailAsync(user.Email, "Reset your password",
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