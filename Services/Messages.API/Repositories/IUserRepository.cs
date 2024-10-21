using Messages.API.Models;

namespace Messages.API.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Get the user by the username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<UserBasic?> GetUserByUsernameAsync(string username);
}