using Microsoft.AspNetCore.Identity;

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
        services.AddDbContext<StepContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            // options.UseNpgsql(configuration.GetConnectionString("DockerConnection"));
        });


        services.AddFluentValidationAutoValidation();
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

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
                    []
                }
            });
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
        services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));
        services.Configure<CloudinaryHelper>(configuration.GetSection("CloudinaryData"));
        services.AddScoped<IRequestUserProvider, RequestUserProvider>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<LogUserActivity>();

        services.AddIdentity<User, IdentityRole>(options => { options.User.RequireUniqueEmail = false; })
            .AddEntityFrameworkStores<StepContext>()
            .AddDefaultTokenProviders();

        var awsSetting = configuration.GetSection("AWS");
        var credentials = new BasicAWSCredentials(awsSetting["AccessKey"], awsSetting["Secret"]);
        var awsOptions = configuration.GetAWSOptions();
        awsOptions.Credentials = credentials;
        awsOptions.Region = RegionEndpoint.EUNorth1;
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IBucketService, BucketService>();
        services.AddSingleton<IBlackListService, BlackListService>();
        services.AddSingleton<BlackListMiddleware>();

        JwtConfig jwtConfig = new();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);

        services.AddSignalR();
        services.AddSingleton<PresenceTracker>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
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


        services.AddAuthorizationBuilder()
            .AddPolicy("CanTest", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new CanTestRequirement());
            });

        return services;
    }
}