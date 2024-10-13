using AuthMiddleware.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Users.API.Data;
using Users.API.Filters;
using Users.API.Mappings;
using Users.API.Repositories;
using Users.API.Services;
using Users.API.Shared;

namespace Users.API.Extensions;

public static class DiExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<UserContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
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
        services.AddScoped<LogUserActivity>();
        services.RegisterJwt(configuration);
        // services.AddSingleton<JwtMiddleware>();

        return services;
    }
}