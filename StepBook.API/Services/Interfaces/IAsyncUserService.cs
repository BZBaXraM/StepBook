using StepBook.API.Models;

namespace StepBook.API.Services.Interfaces;

/// <summary>
/// Interface for the User Service
/// </summary>
public interface IAsyncUserService
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task UpdateUserAsync(User user);
    Task<User> GetUserByUserNameAsync(string username);
    Task<bool> SaveAllAsync();
    Task<User> GetUserByIdAsync(int id);
}