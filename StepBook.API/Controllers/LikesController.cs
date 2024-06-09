namespace StepBook.API.Controllers;

/// <summary>
/// Controller for likes
/// </summary>
/// <param name="userService"></param>
/// <param name="likesService"></param>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LikesController(IAsyncUserService userService, IAsyncLikesService likesService) : ControllerBase
{
    /// <summary>
    /// Add a like
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPost("{username}")]
    public async Task<ActionResult> AddLikeAsync(string username)
    {
        var sourceUserId = User.GetUserId();
        var likedUser = await userService.GetUserByUserNameAsync(username);
        var sourceUser = await likesService.GetUserWithLikesAsync(sourceUserId);

        if (likedUser is null) return NotFound();

        if (sourceUser.UserName == username)
            return BadRequest("You cannot like yourself");


        var userLike = await likesService.GetUserLikeAsync(sourceUserId, likedUser.Id);

        if (userLike != null)
            return BadRequest("You already like this user");


        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            LikedUserId = likedUser.Id
        };

        sourceUser.LikedUsers.Add(userLike);

        if (await userService.SaveAllAsync()) return Ok();

        return BadRequest("Failed to like user");
    }

    /// <summary>
    /// Get a user's likes
    /// </summary>
    /// <param name="likeParams"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikesAsync([FromQuery] LikeParams likeParams)
    {
        likeParams.UserId = User.GetUserId();

        var users = await likesService.GetUserLikesAsync(likeParams);

        Response.AddPagination(users.CurrentPage,
            users.PageSize,
            users.TotalCount,
            users.TotalPages);

        return Ok(users);
    }
}