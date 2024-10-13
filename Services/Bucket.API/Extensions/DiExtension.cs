using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using AuthMiddleware.Jwt;
using Bucket.API.Features.Bucket;
using Microsoft.OpenApi.Models;

namespace Bucket.API.Extensions;

public static class DiExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var awsSetting = configuration.GetSection("AWS");
        var credentials = new BasicAWSCredentials(awsSetting["AccessKey"], awsSetting["Secret"]);
        var awsOptions = configuration.GetAWSOptions();
        awsOptions.Credentials = credentials;
        awsOptions.Region = RegionEndpoint.EUNorth1;
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IBucketService, BucketService>();


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
        services.RegisterJwt(configuration);
        // services.AddSingleton<JwtMiddleware>();

        return services;
    }
}