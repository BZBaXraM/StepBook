namespace StepBook.API.Controllers;

/// <summary>
/// The users controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IAsyncUserService userService, IMapper mapper, IAsyncPhotoService photoService)
    : ControllerBase
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersAsync()
        => Ok(mapper.Map<IEnumerable<MemberDto>>(await userService.GetMembersAsync()));

    /// <summary>
    /// Get a user by their id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        => Ok(await userService.GetUserByIdAsync(id));

    /// <summary>
    /// Get a user by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}", Name = "GetUserByUserName")]
    public async Task<ActionResult<MemberDto>> GetUserByUserName(string username)
        => Ok(mapper.Map<MemberDto>(await userService.GetMemberAsync(username)));

    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> CreateUserAsync(User user)
    {
        await userService.UpdateUserAsync(user);
        return NoContent();
    }

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

        var user = await userService.GetUserByUserNameAsync(User.GetUsername());
        if (user is null)
        {
            return NotFound("User not found");
        }

        mapper.Map(dto, user);
        await userService.UpdateUserAsync(user);

        if (await userService.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update user");
    }

    /// <summary>
    /// Add a photo to a user
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhotoAsync([FromForm] IFormFile file)
    {
        if (string.IsNullOrEmpty(User.GetUsername()))
        {
            return BadRequest("Username claim not found");
        }

        var user = await userService.GetUserByUserNameAsync(User.GetUsername());
        if (user is null)
        {
            return NotFound("User not found");
        }

        var result = await photoService.AddPhotoAsync(file);
        if (result.Error is null)
        {
            return BadRequest(result.Error.Message);
        }

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        user.Photos.Add(photo);

        if (await userService.SaveAllAsync())
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

        var user = await userService.GetUserByUserNameAsync(User.GetUsername());
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

        if (await userService.SaveAllAsync()) return NoContent();

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

        var user = await userService.GetUserByUserNameAsync(User.GetUsername());
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
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error is not null)
            {
                return BadRequest(result.Error.Message);
            }
        }

        user.Photos.Remove(photo);

        if (await userService.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to delete photo");
    }
}