using BuildingBlocks.Shared;
using Messages.API.Models;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;

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