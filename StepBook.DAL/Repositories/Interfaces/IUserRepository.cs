namespace StepBook.DAL.Repositories.Interfaces;

/// <summary>
/// Interface for the User Service
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Update the user
    /// </summary>
    /// <param name="user"></param>
    void Update(User user);

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<User>> GetUsersAsync();

    /// <summary>
    /// Get the user by the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    /// Get the user by the username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<User?> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Get the members
    /// </summary>
    /// <param name="pageParams"></param>
    /// <returns></returns>
    Task<PageList<MemberDto>> GetMembersAsync(PageParams pageParams);

    /// <summary>
    /// Get the member by the username and if it is the current user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="isCurrentUser"></param>
    /// <returns></returns>
    Task<MemberDto?> GetMemberAsync(string username, bool isCurrentUser);

    /// <summary>
    /// Get the user by the photo id
    /// </summary>
    /// <param name="photoId"></param>
    /// <returns></returns>
    Task<User?> GetUserByPhotoId(int photoId);
}