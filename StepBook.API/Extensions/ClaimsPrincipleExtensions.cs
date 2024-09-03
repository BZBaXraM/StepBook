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
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? throw new Exception("Cannot get username from token");

        return username;
    }

    /// <summary>
    /// Get the email from the claims principle
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email)
                    ?? throw new Exception("Cannot get email from token");

        return email;
    }

    /// <summary>
    /// Get the user id from the claims principle
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? throw new Exception("Cannot get username from token"));

        return userId;
    }
}