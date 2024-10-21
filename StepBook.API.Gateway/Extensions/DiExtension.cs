using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StepBook.API.Gateway.Configs;
using StepBook.API.Gateway.Middleware;
using StepBook.API.Gateway.Services;

namespace StepBook.API.Gateway.Extensions;

public static class DiExtension
{
    public static IServiceCollection AuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IJwtService, JwtService>();
        JwtConfig jwtConfig = new();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);
        services.AddSingleton<JwtMiddleware>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        context.Token = string.IsNullOrEmpty(accessToken) switch
                        {
                            false when path.StartsWithSegments("/hubs") => accessToken,
                            _ => context.Token
                        };

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}