namespace StepBook.API.Extensions;

/// <summary>
/// Claims principle extensions
/// </summary>
public static class ClaimsPrincipleExtensions
{
    /// <summary>
    /// Get the username from the claims principle
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.Name)
                       ?? throw new Exception("Cannot get username from token");

        return username;
    }

    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? throw new Exception("Cannot get username from token"));

        return userId;
    }
}