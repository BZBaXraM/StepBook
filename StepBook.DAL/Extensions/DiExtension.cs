namespace StepBook.DAL.Extensions;

public static class DiExtension
{
    public static IServiceCollection RegisterDal(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<StepContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            // options.UseNpgsql(configuration.GetConnectionString("DockerConnection"));
            // options.UseNpgsql(configuration.GetConnectionString("Azure"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();

        return services;
    }
}