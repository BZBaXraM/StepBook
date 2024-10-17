using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using StepBook.Domain.Entities;

namespace Messages.API.Services;

public class AccountService(
    HttpClient client,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AccountService> logger)
{
    // public async Task<User?> GetUserByUsernameAsync(string username)
    // {
    //     var token =
    //         "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJiYXhyYW05NyIsIm5iZiI6MTcyOTIwMjg4NywiZXhwIjoxNzI5NjM0ODg3LCJpYXQiOjE3MjkyMDI4ODd9.MV20_FvoQh0lxNWTOXhfpWC7V3PDynB9mXkBAG2qosp9cEuqFQGMBQHd2-F-Nqr5ixa-G609M_h3t4Gt5UPxlg";
    //     if (string.IsNullOrEmpty(token))
    //     {
    //         logger.LogWarning("Token is null or empty");
    //         return null;
    //     }
    //
    //     logger.LogInformation("Token: {Token}", token);
    //     logger.LogInformation("Fetching user {Username} from API", username);
    //
    //     var user = await GetUserFromApi(username, token);
    //     if (user == null)
    //     {
    //         logger.LogWarning("User {Username} not found", username);
    //     }
    //
    //     return user;
    // }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            logger.LogWarning("Token is null or empty");
            return null;
        }

        logger.LogInformation("Token: {Token}", token);
        logger.LogInformation("Fetching user {Username} from API", username);

        var user = await GetUserFromApi(username, token);
        if (user == null)
        {
            logger.LogWarning("User {Username} not found", username);
        }

        return user;
    }

    private async Task<string?> GetTokenAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        return await Task.FromResult(httpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
    }

    private async Task<User?> GetUserFromApi(string username, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"http://localhost:5000/api/users/{username}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning("User not found");
            return null!;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return user ?? throw new Exception("Cannot deserialize user");
    }
}