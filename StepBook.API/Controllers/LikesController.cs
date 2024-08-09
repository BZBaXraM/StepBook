using StepBook.API.Contracts.Interfaces;

namespace StepBook.API.Controllers;


/// <summary>
/// Likes controller
/// </summary>
/// <param name="unitOfWork"></param>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LikesController(IUnitOfWork unitOfWork) : ControllerBase
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
        var likedUser = await unitOfWork.UserRepository.GetUserByUserNameAsync(username);
        var sourceUser = await unitOfWork.LikeRepository.GetUserWithLikesAsync(sourceUserId);

        if (likedUser is null) return NotFound();

        if (sourceUser.UserName == username)
            return BadRequest("You cannot like yourself");


        var userLike = await unitOfWork.LikeRepository.GetUserLikeAsync(sourceUserId, likedUser.Id);

        if (userLike != null)
            return BadRequest("You already like this user");


        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            LikedUserId = likedUser.Id
        };

        sourceUser.LikedUsers.Add(userLike);

        if (await unitOfWork.UserRepository.SaveAllAsync()) return Ok();

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

        var users = await unitOfWork.LikeRepository.GetUserLikesAsync(likeParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}