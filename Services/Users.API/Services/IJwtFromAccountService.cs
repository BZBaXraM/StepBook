namespace Users.API.Services;

public interface IJwtFromAccountService
{
    Task<string> GetJwtAsync(string usernameOrEmail, string password);
}