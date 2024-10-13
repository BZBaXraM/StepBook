namespace Account.API.Middleware;

/// <summary>
/// The black list middleware
/// </summary>
public class BlackListMiddleware(IBlackListService blackListService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? token = context.Request.Headers.Authorization;

        token = token?.Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(token))
        {
            await next(context);
            return;
        }

        if (blackListService.IsTokenBlackListed(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await next(context);
    }
}