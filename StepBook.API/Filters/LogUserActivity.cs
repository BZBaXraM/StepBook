using StepBook.API.Repositories.Interfaces;

namespace StepBook.API.Filters;

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

        if (!resultContext.HttpContext.User.Identity!.IsAuthenticated) return;
        var userId = resultContext.HttpContext.User.GetUserId();

        var userService = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
        var user = await userService!.GetUserByIdAsync(userId);

        user.LastActive = DateTime.UtcNow;

        await userService.SaveAllAsync();
    }
}