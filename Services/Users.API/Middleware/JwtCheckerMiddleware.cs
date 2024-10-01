namespace Users.API.Middleware;

public class JwtCheckerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string requestPath = context.Request.Path;
        if (requestPath.Contains("account/login", StringComparison.InvariantCultureIgnoreCase))
        {
            await next(context);
        }
        else
        {
            var authorizationHeader = context.Request.Headers.Authorization;
            if (authorizationHeader.FirstOrDefault() == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
            else
            {
                await next(context);
            }
        }
    }
}