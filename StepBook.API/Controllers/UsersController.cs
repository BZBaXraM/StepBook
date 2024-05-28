using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepBook.API.DTOs;
using StepBook.API.Services.Interfaces;

namespace StepBook.API.Controllers;

/// <summary>
/// The users controller
/// </summary>
/// <param name="userService"></param>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IAsyncUserService userService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// The user service
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
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
    /// Update a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> UpdateUser(User user)
    {
        await userService.UpdateUserAsync(user);
        return NoContent();
    }

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
}