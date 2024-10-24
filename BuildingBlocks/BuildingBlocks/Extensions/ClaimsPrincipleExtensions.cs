using System.Security.Claims;

namespace BuildingBlocks.Extensions;

public static class ClaimsPrincipleExtensions
{
    /// <summary>
    /// Get the username from the claims principle
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        // return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return user.FindFirst(ClaimTypes.Name)?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    /// <summary>
    /// Get the email from the claims principle
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string? GetEmail(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Email)?.Value;

    /// <summary>
    /// Get the user id from the claims principle 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static int GetUserId(this ClaimsPrincipal user)
        => int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
}