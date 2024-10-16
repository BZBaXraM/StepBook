using System.Text.Json;
using StepBook.Domain.DTOs;
using StepBook.Domain.Entities;

namespace Messages.API.Services;

public class AccountService(HttpClient client)
{
    /// <summary>
    /// Get user by username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var response = await client.GetAsync($"http://localhost:5004/api/Users/{username}");

        if (!response.IsSuccessStatusCode) return null!;

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}