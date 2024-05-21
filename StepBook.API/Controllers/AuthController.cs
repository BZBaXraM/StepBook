using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StepBook.API.Data;
using StepBook.API.Data.Entities;
using StepBook.API.DTOs;
using StepBook.API.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using StepBook.API.Models;

namespace StepBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        StepContext context,
        IJwtService jwtService,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
        : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register(RegisterDto dto)
        {
            var existingUser = await userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
            {
                return Conflict("User with the same email already exists");
            }

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var appUser = new AppUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                RefreshToken = Guid.NewGuid().ToString("N").ToLower()
            };

            var result = await userManager.CreateAsync(appUser, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return await GenerateToken(appUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return NotFound("User not found");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid password");
            }

            return await GenerateToken(user);
        }

        private async Task<LoginResponseDto> GenerateToken(AppUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            var accessToken = jwtService.GenerateSecurityToken(user.Id, user.UserName!, roles, userClaims);
            var refreshToken = user.RefreshToken;

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken!
            };
        }
    }
}