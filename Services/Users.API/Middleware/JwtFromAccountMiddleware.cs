using Users.API.Services;

namespace Users.API.Middleware;

public class JwtFromAccountMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Resolve IJwtFromAccountService from the request scope
        var jwtFromAccountService = context.RequestServices.GetRequiredService<IJwtFromAccountService>();

        var usernameOrEmail = context.Request.Headers["UsernameOrEmail"];
        var password = context.Request.Headers["Password"];

        if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Bad request");
            return;
        }

        var jwt = await jwtFromAccountService.GetJwtAsync(usernameOrEmail, password);

        if (jwt == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        context.Response.Headers.Append("Authorization", $"Bearer {jwt}");

        await next(context);
    }
}