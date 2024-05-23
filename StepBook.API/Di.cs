using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StepBook.API.Auth;
using StepBook.API.Data;
using StepBook.API.Data.Entities;
using StepBook.API.Providers;
using StepBook.API.Services.Classes;
using StepBook.API.Services.Interfaces;

namespace StepBook.API;

/// <summary>
/// Dependency injection extension methods.
/// </summary>
public static class Di
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "StepBook.API", Version = "v1" });

            var path = Path.Combine(AppContext.BaseDirectory, "StepBook.API.xml");
            c.IncludeXmlComments(path);
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
                    Array.Empty<string>()
                }
            });
        });

        services.AddDbContext<StepContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddDbContext<AuthContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("IdentityConnection"));
        });


        return services;
    }

    /// <summary>
    /// Add services to the container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IRequestUserProvider, RequestUserProvider>();
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<IJwtService, JwtService>();

        JwtConfig jwtConfig = new();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("CanTest", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new CanTestRequirement());
            });

        return services;
    }
}