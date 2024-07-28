namespace StepBook.API.Repositories.Interfaces;

/// <summary>
/// Interface for the User Service
/// </summary>
public interface IUserRepository
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
    void UpdateUser(User user);

    /// <summary>
    /// Get a user by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<User> GetUserByUserNameAsync(string? username);

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

    /// <summary>
    /// Get all members
    /// </summary>
    /// <returns></returns>
    Task<PageList<MemberDto>> GetMembersAsync(PageParams pageParams);

    /// <summary>
    /// Get a member by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<MemberDto> GetMemberAsync(string username);
}