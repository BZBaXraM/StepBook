using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using StepBook.Domain.Entities;

namespace Messages.API.Services;

public class AccountService(
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AccountService> logger)
{
    // public async Task<User?> GetUserByUsernameAsync(string username)
    // {
    //     var token =
    //         "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJiYXhyYW05NyIsIm5iZiI6MTcyOTI1MTgzNywiZXhwIjoxNzI5NjgzODM3LCJpYXQiOjE3MjkyNTE4Mzd9.tizsEhxD7DJaQVeY2lWQl7tjp1hRExg9qjhWu2od3btDZz6Du-Ka_s87mYcedEA51VIIVl4NmVLb4PTo8LwiOw";
    //     if (string.IsNullOrEmpty(token))
    //     {
    //         logger.LogWarning("Token is null or empty");
    //         return null;
    //     }
    //
    //     logger.LogInformation("Token: {Token}", token);
    //     logger.LogInformation("Fetching user {Username} from API", username);
    //
    //     var user = await GetUserFromApiAsync(username, token);
    //     if (user == null)
    //     {
    //         logger.LogWarning("User {Username} not found", username);
    //     }
    //
    //     return user;
    // }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var token = httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
        {
            logger.LogWarning("Token is null or not in correct format");
            return null;
        }

        token = token["Bearer ".Length..].Trim(); // Извлечение токена
        logger.LogInformation("Token: {Token}", token);
        logger.LogInformation("Fetching user {Username} from API", username);

        var user = await GetUserFromApiAsync(username, token);
        if (user == null)
        {
            logger.LogWarning("User {Username} not found", username);
        }

        return user;
    }

    private async Task<User?> GetUserFromApiAsync(string username, string token)
    {
        var client = httpClientFactory.CreateClient("Account.API");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"api/users/{username}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return user;
    }
}