using Account.API.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepBook.DatabaseLayer.Data;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;

namespace Account.API.Features.Account;

[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
public class AccountController(
    StepContext context,
    IMapper mapper,
    IEmailService emailService,
    IJwtService jwtService,
    IBlackListService blackListService) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] RegisterRequest request)
    {
        try
        {
            var user = mapper.Map<User>(request);
            user.UserName = request.Username;

            user.Password = PasswordHash(request.Password);

            if (await context.Users.AnyAsync(x => x.Email == user.Email))
            {
                return BadRequest("Email already exists");
            }

            if (await context.Users.AnyAsync(x => x.UserName == user.UserName))
            {
                return BadRequest("Username already exists");
            }

            if (!ModelState.IsValid) return BadRequest();

            if (!PasswordVerify(request.Password, user.Password))
            {
                return BadRequest("Invalid password");
            }

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
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing your request. {ex.Message}");
        }
    }

    /// <summary>
    /// Login a user
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginAsync([FromBody] LoginRequest request)
    {
        var user = await context.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(x => x.UserName == request.UsernameOrEmail || x.Email == request.UsernameOrEmail);

        if (user == null || !user.IsEmailConfirmed)
        {
            return Unauthorized("Invalid username, email, or email not confirmed.");
        }

        if (!PasswordVerify(request.Password, user.Password))
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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Password = PasswordHash(request.CurrentPassword);

        if (!PasswordVerify(request.CurrentPassword, user.Password))
        {
            return BadRequest("Invalid password");
        }

        if (request.NewPassword != request.ConfirmNewPassword)
        {
            return BadRequest("Passwords do not match");
        }

        user.Password = PasswordHash(request.NewPassword);


        await context.SaveChangesAsync();

        return Ok("Password changed successfully");
    }

    /// <summary>
    /// Change the username of a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("change-username")]
    [Authorize]
    public async Task<ActionResult> ChangeUsernameAsync([FromBody] ChangeUsernameRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

        if (user == null)
        {
            return NotFound("User not found");
        }

        user.UserName = request.NewUsername;
        await context.SaveChangesAsync();

        return Ok("Username changed successfully");
    }

    /// <summary>
    ///     Forget the password of a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forget-password")]
    public async Task<ActionResult> ForgotPasswordAsync([FromBody] ForgetUserPasswordRequest request)
    {
        if (!ModelState.IsValid) return BadRequest();

        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }

        if (user.RandomCode != request.Code)
        {
            return BadRequest("Invalid reset code");
        }

        user.Password = PasswordHash(request.NewPassword);
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

    private string PasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool PasswordVerify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}