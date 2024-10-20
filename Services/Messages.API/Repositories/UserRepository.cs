using AutoMapper;
using Messages.API.Models;
using Messages.API.Services;

namespace Messages.API.Repositories;

public class UserRepository(UserService userService, IMapper mapper) : IUserRepository
{
    public async Task<UserBasic?> GetUserByUsernameAsync(string username)
    {
        var user = await userService.GetUserByUsernameAsync(username);
        return mapper.Map<UserBasic>(user);
    }
}