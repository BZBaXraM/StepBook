using AuthMiddleware.Jwt;
using Microsoft.AspNetCore.RateLimiting;
using StepBook.API.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

builder.Services.AuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddCors();

var app = builder.Build();

// app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

app.UseRateLimiter();

app.MapReverseProxy();

app.Run();