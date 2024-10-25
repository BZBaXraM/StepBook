using StepBook.BuildingBlocks.Extensions;
using StepBook.BuildingBlocks.Shared;
using StepBook.DAL.Contracts.Interfaces;
using StepBook.DAL.Entities;
using StepBook.DTO.DTOs;

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
    /// Toggle like - like or unlike a user
    /// </summary>
    /// <param name="targetUserId"></param>
    /// <returns></returns>
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        var sourceUserId = User.GetUserId();

        if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

        var existingLike = await unitOfWork.LikeRepository.GetUserLike(sourceUserId, targetUserId);

        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId
            };

            await unitOfWork.LikeRepository.AddLikeAsync(like);
        }
        else
        {
            unitOfWork.LikeRepository.DeleteLike(existingLike);
        }

        if (await unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to update like");
    }

    /// <summary>
    /// Get the current user's like ids
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await unitOfWork.LikeRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }

    /// <summary>
    /// Get the users that the current user likes
    /// </summary>
    /// <param name="likesParams"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await unitOfWork.LikeRepository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}