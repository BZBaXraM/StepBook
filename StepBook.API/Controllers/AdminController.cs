namespace StepBook.API.Controllers;

/// <summary>
/// The admin controller
/// </summary>
[Authorize(Policy = "AdminOnly")]
[Route("api/[controller]")]
[ApiController]
public class AdminController(StepContext context) : ControllerBase
{
    /// <summary>
    /// Add a user to the blacklist
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPost("add-to-blacklist/{username}")]
    public async Task<ActionResult> AddToBlackListAsync([FromRoute] string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var currentUser = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name!);
        if (currentUser is null)
        {
            return NotFound("User not found");
        }

        var blackListedUser = new BlackListedUser
        {
            BlackListedUserId = user.Id,
            BlackList = user,
            UserId = currentUser.Id,
            User = currentUser
        };

        currentUser.BlackListedUsers.Add(blackListedUser);

        if (await context.SaveChangesAsync() > 0) return NoContent();

        return BadRequest("Failed to add user to blacklist");
    }

    /// <summary>
    /// Remove a user from the blacklist
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPost("remove-from-blacklist/{username}")]
    public async Task<ActionResult> RemoveFromBlackListAsync([FromRoute] string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var currentUser = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name!);
        if (currentUser is null)
        {
            return NotFound("User not found");
        }

        var blackListedUser =
            await context.BlackListedUsers.FirstOrDefaultAsync(x =>
                x.UserId == currentUser.Id && x.BlackListedUserId == user.Id);
        if (blackListedUser is null)
        {
            return BadRequest("User not in blacklist");
        }

        currentUser.BlackListedUsers.Remove(blackListedUser);

        if (await context.SaveChangesAsync() > 0) return NoContent();

        return BadRequest("Failed to remove user from blacklist");
    }

    /// <summary>
    /// Get the blacklist
    /// </summary>
    /// <returns></returns>
    [HttpGet("blacklist")]
    public async Task<ActionResult<IEnumerable<BlackListedUser>>> GetBlackListAsync()
    {
        var currentUser = await context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name!);
        if (currentUser is null)
        {
            return NotFound("User not found");
        }

        var blackListedUsers = await context.BlackListedUsers
            .Where(x => x.UserId == currentUser.Id)
            .Select(x => x.BlackList)
            .ToListAsync();

        return Ok(blackListedUsers);
    }
}