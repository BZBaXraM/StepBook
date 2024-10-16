using BuildingBlocks.Extensions;
using BuildingBlocks.Shared;
using Likes.API.Data;
using Likes.API.Extensions;
using Likes.API.Filters;
using Likes.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;

namespace Likes.API.Features.Likes;

/// <summary>
/// Likes controller
/// </summary>
// [ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")] // api/likes
[ApiController]
[Authorize]
public class LikesController(ILikesRepository repository, LikeContext context) : ControllerBase
{
    /// <summary>
    /// Toggle like - like or unlike a user
    /// </summary>
    /// <param name="targetUserId"></param>
    /// <returns></returns>
    [HttpPost("{targetUserId:int}")] // api/likes/{targetUserId}
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        var sourceUserId = User.GetUserId();

        if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

        var existingLike = await repository.GetUserLike(sourceUserId, targetUserId);

        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId
            };

            await repository.AddLikeAsync(like);
        }
        else
        {
            repository.DeleteLike(existingLike);
        }

        if (await context.SaveChangesAsync() > 0) return Ok();

        return BadRequest("Failed to update like");
    }

    /// <summary>
    /// Get the current user's like ids
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")] // api/likes/list
    public async Task<ActionResult<List<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await repository.GetCurrentUserLikeIds(User.GetUserId()));
    }

    /// <summary>
    /// Get the users that the current user likes
    /// </summary>
    /// <param name="likesParams"></param>
    /// <returns></returns>
    [HttpGet] // api/likes
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();

        var users = await repository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}