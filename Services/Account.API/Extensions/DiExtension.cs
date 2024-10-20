using System.Text;
using Account.API.Features.Account;
using Account.API.Repositories.Classes;
using Account.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BlackListMiddleware = Account.API.Middleware.BlackListMiddleware;
using JwtMiddleware = Account.API.Middleware.JwtMiddleware;

namespace Account.API.Extensions;

public static class DiExtension // StepBook.API/Extensions/DiExtension.cs - from Account.API
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<AccountContext>(options =>
        {
            // options.UseNpgsql(configuration.GetConnectionString("Database"));
            options.UseNpgsql(configuration.GetConnectionString("Local"));
        });

        services.Configure<CloudinaryHelper>(configuration.GetSection("CloudinaryData"));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhotoService, PhotoService>();

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
        services.AddSingleton<BlackListMiddleware>();
        services.AddSingleton<JwtMiddleware>();

        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<IJwtService, JwtService>();
        JwtConfig jwtConfig = new();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);

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