using Account.API.Data;
using Account.API.Features.Account;
using Account.API.Filters;
using Account.API.Mappings;
using Account.API.Providers;
using Account.API.Services;
using Account.API.Shared;
using AuthMiddleware.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BlackListMiddleware = Account.API.Middleware.BlackListMiddleware;
using BlackListService = Account.API.Features.Account.BlackListService;
using IBlackListService = Account.API.Features.Account.IBlackListService;

namespace Account.API.Extensions;

public static class DiExtension // StepBook.API/Extensions/DiExtension.cs - from Account.API
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<AccountContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });


        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "StepBook.API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }

    public static IServiceCollection AuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));
        services.AddScoped<IRequestUserProvider, RequestUserProvider>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddScoped<LogUserActivity>();

        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<BlackListMiddleware>();
        services.RegisterJwt(configuration);
        services.AddSingleton<JwtMiddleware>();

        return services;
    }
}