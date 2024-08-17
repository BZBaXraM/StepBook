using System.Net;
using System.Text.Json;
using StepBook.API.Exceptions;

namespace StepBook.API.Middleware;

/// <summary>
///  Middleware for handling exceptions.
/// </summary>
/// <param name="next"></param>
/// <param name="logger"></param>
/// <param name="env"></param>
public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env)
{
    /// <summary>
    /// Invoke the middleware.
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}