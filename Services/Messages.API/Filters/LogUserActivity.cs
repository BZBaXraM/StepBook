using Messages.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Messages.API.Fillters;

/// <summary>
/// Log the user activity filter
/// </summary>
public class LogUserActivity : IAsyncActionFilter
{
    /// <summary>
    /// Log the user activity
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

        var userId = resultContext.HttpContext.User.GetUserId();

        var user = await messageContext.Users.FindAsync(userId);

        if (user == null) return;

        user.LastActive = DateTime.UtcNow;

        await likeContext.SaveChangesAsync();
    }
}