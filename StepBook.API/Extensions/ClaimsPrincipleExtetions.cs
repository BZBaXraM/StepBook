namespace StepBook.API.Extensions;

public static class ClaimsPrincipleExtetions
{
    public static string GetUsername(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Name)?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}