using StepBook.BLL.Services;
using StepBook.DAL.Data;

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
    public async Task<ActionResult<string>> RegisterAsync([FromBody] RegisterDto dto)
    {
        var user = mapper.Map<User>(dto);
        user.UserName = dto.Username;

        if (await context.Users.AnyAsync(x => x.Email == user.Email))
        {
            return BadRequest("Email already exists");
        }

        if (await context.Users.AnyAsync(x => x.UserName == user.UserName))
        {
            return BadRequest("Username already exists");
        }

        user.Password = PasswordHash(dto.Password);

        if (!ModelState.IsValid) return BadRequest();

        // Генерация кода подтверждения
        var confirmationCode = GenerateRandomCode();
        user.EmailConfirmationCode = confirmationCode;

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        await emailService.SendEmailAsync(user.Email, "Confirm your email",
            $"Your confirmation code is: {confirmationCode}");


        return Ok("Registration successful. Please check your email for the confirmation code.");
    }

    /// <summary>
    /// Login a user
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginAsync([FromBody] LoginDto dto)
    {
        var user = await context.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(x => x.UserName == dto.UsernameOrEmail || x.Email == dto.UsernameOrEmail);

        if (user == null || !user.IsEmailConfirmed)
        {
            return Unauthorized("Invalid username, email, or email not confirmed.");
        }

        // if user is blacklisted
        if (await context.BlackListedUsers.AnyAsync(x => x.BlackListedUserId == user.Id))
        {
            return Unauthorized("You are blacklisted");
        }

        if (!PasswordVerify(dto.Password, user.Password))
        {
            return Unauthorized("Invalid password");
        }

        user.RefreshToken = jwtService.GenerateRefreshToken();
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
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync([FromBody] TokenDto dto)
    {
        blackListService.AddTokenToBlackList(dto.Token);
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
    /// Confirm the email of a user using a confirmation code
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("confirm-email-code")]
    public async Task<ActionResult> ConfirmEmailCodeAsync([FromBody] ConfirmEmailCodeDto dto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.EmailConfirmationCode == dto.Code);

        if (user == null)
        {
            return NotFound("User not found");
        }

        user.IsEmailConfirmed = true;
        await context.SaveChangesAsync();

        return Ok("Email confirmed successfully. You can now log in.");
    }

    /// <summary>
    /// Resend the confirmation code to the email of a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("resend-confirmation-code")]
    public async Task<ActionResult> ResendConfirmationCodeAsync([FromBody] ResendConfirmationCodeDto dto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null)
        {
            return NotFound("User not found");
        }

        var confirmationCode = GenerateRandomCode();
        user.EmailConfirmationCode = confirmationCode;
        await context.SaveChangesAsync();

        await emailService.SendEmailAsync(user.Email, "Confirm your email",
            $"Your confirmation code is: {confirmationCode}");

        return Ok("Confirmation code sent to your email");
    }


    /// <summary>
    /// Change the password of a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Password = PasswordHash(dto.CurrentPassword);

        if (!PasswordVerify(dto.CurrentPassword, user.Password))
        {
            return BadRequest("Invalid password");
        }

        if (dto.NewPassword != dto.ConfirmNewPassword)
        {
            return BadRequest("Passwords do not match");
        }

        user.Password = PasswordHash(dto.NewPassword);


        await context.SaveChangesAsync();

        return Ok("Password changed successfully");
    }

    /// <summary>
    /// Change the username of a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("change-username")]
    [Authorize]
    public async Task<ActionResult> ChangeUsernameAsync([FromBody] ChangeUsernameRequestDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

        if (user == null)
        {
            return NotFound("User not found");
        }

        user.UserName = dto.NewUsername;
        await context.SaveChangesAsync();

        return Ok("Username changed successfully");
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

        user.Password = PasswordHash(dto.NewPassword);
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

    private static string PasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool PasswordVerify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}