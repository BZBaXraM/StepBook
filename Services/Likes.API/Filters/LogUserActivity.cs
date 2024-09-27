using BuildingBlocks.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using StepBook.DatabaseLayer.Data;

namespace Likes.API.Filters;

/// <summary>
/// Log the user activity filter
/// </summary>
public class LogUserActivity(StepContext stepContext) : IAsyncActionFilter
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

        var user = await stepContext.Users.FindAsync(userId);

        if (user == null) return;

        user.LastActive = DateTime.UtcNow;

        await stepContext.SaveChangesAsync();
    }
}