using System.Text;
using Messages.API.Data;
using Messages.API.Filters;
using Messages.API.Hubs;
using Messages.API.Mappings;
using Messages.API.Middleware;
using Messages.API.Repositories;
using Messages.API.Services;
using Messages.API.Shared.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Messages.API.Extensions;
// Services/Messages.API/Extensions/DiExtension.cs

public static class DiExtension
{
    public static IServiceCollection AuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<MessageContext>(options =>
        {
            // options.UseNpgsql(configuration.GetConnectionString("Database"));
            options.UseNpgsql(configuration.GetConnectionString("Local"));
        });

        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddSignalR();

        services.AddSingleton<PresenceTracker>();
        services.AddScoped<LogUserActivity>();
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