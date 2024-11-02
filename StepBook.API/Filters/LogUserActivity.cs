using StepBook.DAL.Contracts.Interfaces;

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

        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

        var userId = resultContext.HttpContext.User.GetUserId();

        var unitOfWork = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

        if (user == null) return;

        user.LastActive = DateTime.UtcNow;

        await unitOfWork.Complete();
    }
}