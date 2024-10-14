using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AuthMiddleware.Jwt;

public class JwtMiddleware(IJwtService jwtService, ILogger<JwtMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader["Bearer ".Length..].Trim();

            logger.LogInformation("Authorization header: {AuthHeader}, Extracted token: {Token}", authHeader, token);

            if (!string.IsNullOrWhiteSpace(token))
            {
                var tokenSegments = token.Split('.').Length;
                if (tokenSegments is 3 or 5)
                {
                    AttachUserToContext(context, token);
                }
                else
                {
                    logger.LogWarning("Invalid token format");
                }
            }
            else
            {
                logger.LogWarning("Token is null or empty");
            }
        }
        else
        {
            logger.LogWarning("Authorization header is missing or not in the correct format");
        }

        await next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var principal = jwtService.GetPrincipalFromToken(token);
            if (principal != null)
            {
                context.User = principal;
                logger.LogInformation("User attached to context {Name}", context.User.Identity!.Name);
                Console.WriteLine($"User attached to context {context.User.Identity.Name}");
            }
            else
            {
                logger.LogWarning("Principal is null");
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error attaching user to context {ExMessage}", ex.Message);
            Console.WriteLine($"Error attaching user to context {ex.Message}");
        }
    }
}