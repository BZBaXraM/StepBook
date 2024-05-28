using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepBook.API.Data;
using StepBook.API.DTOs;
using StepBook.API.Services.Interfaces;

namespace StepBook.API.Controllers;

/// <summary>
/// Account controller
/// </summary>
/// <param name="context"></param>
/// <param name="jwtService"></param>
[Route("api/[controller]")]
[ApiController]
public class AccountController(StepContext context, IJwtService jwtService) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] RegisterDto dto)
    {
        if (await UserExists(dto.Username))
        {
            return BadRequest("Username is already taken");
        }

        using var hmac = new HMACSHA512();

        var user = new User
        {
            UserName = dto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key,
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user)
        };
    }

    /// <summary>
    /// Login a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginAsync([FromBody] RegisterDto dto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == dto.Username);

        if (user == null)
        {
            return Unauthorized("Invalid username");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
        {
            return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user)
        };
    }

    private async Task<bool> UserExists(string username)
        => await context.Users.AnyAsync(x => x.UserName == username);
}