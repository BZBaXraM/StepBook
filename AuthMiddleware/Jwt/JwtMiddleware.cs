using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace AuthMiddleware.Jwt;

public class JwtMiddleware(IJwtService jwtService) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Get the token from the Authorization header
        var bearer = context.Request.Headers.Authorization.ToString();
        var token = bearer.Replace("Bearer ", string.Empty);

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var principal = jwtService.GetPrincipalFromToken(token);
                context.User = principal;
            }
            catch (SecurityTokenException)
            {
                context.Response.StatusCode = 401;
            }
        }

        await next(context);
    }
}