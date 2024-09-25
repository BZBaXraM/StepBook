using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;
using Users.API.Data;
using Users.API.Extensions;
using Users.API.Filters;
using Users.API.Services;
using Users.API.Shared;

namespace Users.API.Features.User;

[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(UserContext context, IUserRepository repository, IMapper mapper, IPhotoService service)
    : ControllerBase
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersAsync([FromQuery] PageParams pageParams)
    {
        pageParams.CurrentUsername = User.GetUsername()!;

        var users = await repository.GetMembersAsync(pageParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }

    /// <summary>
    /// Get a user by their id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StepBook.Domain.Entities.User>> GetUserByIdAsync(int id)
        => Ok(await repository.GetUserByIdAsync(id));

    /// <summary>
    /// Get a user by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}", Name = "GetUserByUserName")]
    public async Task<ActionResult<MemberDto>> GetUserByUserName(string username)
    {
        var currentUsername = User.GetUsername();

        var user = await repository.GetMemberAsync(username,
            isCurrentUser: currentUsername == username);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }


    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> UpdateUserAsync(MemberUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrEmpty(User.GetUsername()))
        {
            return BadRequest("Username claim not found");
        }

        var user = await repository.GetUserByUsernameAsync(User.GetUsername()!);
        if (user is null)
        {
            return NotFound("User not found");
        }

        mapper.Map(dto, user);
        repository.Update(user);

        if (await context.SaveChangesAsync() > 0) return NoContent();
        return BadRequest("Failed to update user");
    }

    /// <summary>
    /// Add a photo to a user
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhotoAsync(IFormFile file)
    {
        var username = User.GetUsername();
        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Username claim not found");
        }

        var user = await repository.GetUserByUsernameAsync(username);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var result = await service.AddPhotoAsync(file);
        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos == null || user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        user.Photos!.Add(photo);

        if (await context.SaveChangesAsync() > 0)
        {
            return CreatedAtRoute("GetUserByUserName", new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("Problem adding photo");
    }


    /// <summary>
    /// Set a photo as the main photo in account
    /// </summary>
    /// <param name="photoId"></param>
    /// <returns></returns>
    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhotoAsync(int photoId)
    {
        if (string.IsNullOrEmpty(User.GetUsername()))
        {
            return BadRequest("Username claim not found");
        }

        var user = await repository.GetUserByUsernameAsync(User.GetUsername()!);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo is null)
        {
            return NotFound("Photo not found");
        }

        if (photo.IsMain)
        {
            return BadRequest("This is already your main photo");
        }

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain is not null)
        {
            currentMain.IsMain = false;
        }

        photo.IsMain = true;

        if (await context.SaveChangesAsync() > 0) return NoContent();

        return BadRequest("Failed to set main photo");
    }

    /// <summary>
    /// Delete a photo from a user
    /// </summary>
    /// <param name="photoId"></param>
    /// <returns></returns>
    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhotoAsync(int photoId)
    {
        if (string.IsNullOrEmpty(User.GetUsername()))
        {
            return BadRequest("Username claim not found");
        }

        var user = await repository.GetUserByUsernameAsync(User.GetUsername()!);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo is null)
        {
            return NotFound("Photo not found");
        }

        if (photo.IsMain)
        {
            return BadRequest("You cannot delete your main photo");
        }

        if (photo.PublicId is not null)
        {
            var result = await service.DeletePhotoAsync(photo.PublicId);
            if (result.Error is not null)
            {
                return BadRequest(result.Error.Message);
            }
        }

        user.Photos.Remove(photo);

        if (await context.SaveChangesAsync() > 0) return NoContent();

        return BadRequest("Failed to delete photo");
    }
}