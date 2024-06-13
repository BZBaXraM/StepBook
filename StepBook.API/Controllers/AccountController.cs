namespace StepBook.API.Controllers;

/// <summary>
/// Account controller
/// </summary>
/// <param name="context"></param>
/// <param name="jwtService"></param>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
public class AccountController(StepContext context, IJwtService jwtService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] RegisterDto dto)
    {
        if (await UserExists(dto.Username, dto.Email)) return BadRequest("Username or Email is already taken");

        var user = mapper.Map<User>(dto);

        using var hmac = new HMACSHA512();

        user.UserName = dto.Username;
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        user.PasswordSalt = hmac.Key;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user),
            KnownAs = user.KnownAs!
        };
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

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
        {
            return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = jwtService.GenerateSecurityToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs!
        };
    }

    private async Task<bool> UserExists(string username, string email)
    {
        return await context.Users.AnyAsync(x => x.UserName == username || x.Email == email);
    }
}