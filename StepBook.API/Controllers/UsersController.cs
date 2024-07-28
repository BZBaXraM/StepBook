using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Controllers;

/// <summary>
/// The users controller
/// </summary>
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoRepository photoRepository)
    : ControllerBase
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersAsync([FromQuery] PageParams pageParams)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        pageParams.CurrentUsername = User.GetUsername();

        if (string.IsNullOrEmpty(pageParams.Gender))
        {
            pageParams.Gender = user.Gender == "male" ? "female" : "male";
        }

        var users = await userRepository.GetMembersAsync(pageParams);
        Response.AddPagination(users.CurrentPage,
            users.PageSize,
            users.TotalCount,
            users.TotalPages);

        return Ok(users);
    }

    /// <summary>
    /// Get a user by their id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        => Ok(await userRepository.GetUserByIdAsync(id));

    /// <summary>
    /// Get a user by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}", Name = "GetUserByUserName")]
    public async Task<ActionResult<MemberDto>> GetUserByUserName(string username)
        => Ok(mapper.Map<MemberDto>(await userRepository.GetMemberAsync(username)));


    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> UpdateUserAsync(MemberUpdateDto dto)
    {
        if (string.IsNullOrEmpty(User.GetUsername()))
        {
            return BadRequest("Username claim not found");
        }

        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        if (user is null)
        {
            return NotFound("User not found");
        }

        mapper.Map(dto, user);
        userRepository.UpdateUser(user);

        if (await userRepository.SaveAllAsync()) return NoContent();
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
        // Check if the username claim is present
        var username = User.GetUsername();
        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("Username claim not found");
        }

        // Fetch the user from the database
        var user = await userRepository.GetUserByUserNameAsync(username);
        if (user is null)
        {
            return NotFound("User not found");
        }

        // Add the photo using the photo service
        var result = await photoRepository.AddPhotoAsync(file);
        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }

        // Create a new Photo object
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        // Check if this is the user's first photo
        if (user.Photos == null || user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        // Add the photo to the user's photo collection
        user.Photos!.Add(photo);

        // Save the changes to the database
        if (await userRepository.SaveAllAsync())
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

        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
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

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to set main photo");
    }

    /// <summary>
    /// Delete a photo from a user
    /// </summary>
    /// <param nam e="photoId"></param>
    /// <returns></returns>
    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhotoAsync(int photoId)
    {
        if (string.IsNullOrEmpty(User.GetUsername()))
        {
            return BadRequest("Username claim not found");
        }

        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
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
            var result = await photoRepository.DeletePhotoAsync(photo.PublicId);
            if (result.Error is not null)
            {
                return BadRequest(result.Error.Message);
            }
        }

        user.Photos.Remove(photo);

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to delete photo");
    }
}