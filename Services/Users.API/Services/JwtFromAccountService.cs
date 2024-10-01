namespace Users.API.Services;

public class JwtFromAccountService : IJwtFromAccountService
{
    public async Task<string> GetJwtAsync(string usernameOrEmail, string password)
    {
        var client = new HttpClient();
        var response = await client.PostAsJsonAsync("https://localhost:6060/api/account/login", new
        {
            usernameOrEmail,
            password
        });

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var jwt = await response.Content.ReadAsStringAsync();

        return jwt;
    }
}