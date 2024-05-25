using Microsoft.EntityFrameworkCore;
using StepBook.API.Data;
using StepBook.API.Models;
using StepBook.API.Services.Interfaces;

namespace StepBook.API.Services.Classes;

/// <summary>
/// Service for the User
/// </summary>
public class UserService(StepContext context) : IAsyncUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<User>> GetUsersAsync()
        => await context.Users.ToListAsync();

    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="user"></param>
    public async Task UpdateUserAsync(User user)
    {
        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a user by their username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<User> GetUserByUserNameAsync(string username)
    {
        return (await context.Users.FirstOrDefaultAsync(x => x.UserName == username))!;
    }

    /// <summary>
    /// Save all changes
    /// </summary>
    /// <returns></returns>
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Get a user by their id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User> GetUserByIdAsync(int id)
    {
        return (await context.Users.FirstOrDefaultAsync(x => x.Id == id))!;
    }
}