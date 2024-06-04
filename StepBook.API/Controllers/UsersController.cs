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
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUserByUserNameAsync(string username)
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
        var userName = User.GetUsername();
        if (string.IsNullOrEmpty(userName))
        {
            return BadRequest("Username claim not found");
        }

        var user = await userService.GetUserByUserNameAsync(userName);
        if (user == null)
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
        var userName = User.GetUsername();
        if (string.IsNullOrEmpty(userName))
        {
            return BadRequest("Username claim not found");
        }

        var user = await userService.GetUserByUserNameAsync(userName);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null)
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
            return Ok(mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("Problem adding photo");
    }
}