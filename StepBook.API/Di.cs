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
        services.AddScoped<PresenceTracker>();
        services.Configure<CloudinaryHelper>(configuration.GetSection("CloudinaryData"));
        services.AddScoped<IRequestUserProvider, RequestUserProvider>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAsyncUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAsyncPhotoService, PhotoService>();
        services.AddScoped<IAsyncLikesService, LikesService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<LogUserActivity>();

        JwtConfig jwtConfig = new();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);

        services.AddSignalR();

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


        services.AddAuthorizationBuilder()
            .AddPolicy("CanTest", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new CanTestRequirement());
            });

        return services;
    }
}