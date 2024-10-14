using AuthMiddleware.Jwt;
using Likes.API.Data;
using Likes.API.Filters;
using Likes.API.Mappings;
using Likes.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Likes.API.Extensions;

public static class DiExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<LikeContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });

        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddScoped<ILikesRepository, LikesRepository>();

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
        services.AddSingleton<JwtMiddleware>();

        return services;
    }
}