using System.Net;
using System.Text;
using System.Text.Json;
using Messages.API.Models;

namespace Messages.API.Services;

public class UserService(
    IHttpClientFactory httpClientFactory,
    ILogger<UserService> logger)
{
    public async Task<UserBasic?> GetUserByUsernameAsync(string username)
    {
        var user = await GetUserFromApiAsync(username);
        if (user == null)
        {
            logger.LogWarning("User {Username} not found", username);
        }

        return user;
    }

    // GetUserByIdAsync
    public async Task<UserBasic?> GetUserByIdAsync(int id)
    {
        var user = await GetUserFromApiAsync(id.ToString());
        if (user == null)
        {
            logger.LogWarning("User {Id} not found", id);
        }

        return user;
    }

    public async Task UpdateUserAsync(UserBasic user)
    {
        var client = httpClientFactory.CreateClient("Account.API");

        var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"api/users/{user.Id}", content);

        response.EnsureSuccessStatusCode();
    }

    private async Task<UserBasic?> GetUserFromApiAsync(string username)
    {
        var client = httpClientFactory.CreateClient("Account.API");

        // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"api/users/{username}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserBasic>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return user;
    }
}