using StepBook.API.DTOs;

namespace StepBook.API.Services.Interfaces;

/// <summary>
/// Interface for the User Service
/// </summary>
public interface IAsyncUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<User>> GetUsersAsync();
    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task UpdateUserAsync(User user);
    /// <summary>
    /// Get a user by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<User> GetUserByUserNameAsync(string username);
    /// <summary>
    /// Save all changes
    /// </summary>
    /// <returns></returns>
    Task<bool> SaveAllAsync();
    /// <summary>
    /// Get a user by their id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto> GetMemberAsync(string username);
}