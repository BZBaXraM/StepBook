using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StepBook.DAL.Contracts.Classes;
using StepBook.DAL.Contracts.Interfaces;
using StepBook.DAL.Data;
using StepBook.DAL.Repositories.Classes;
using StepBook.DAL.Repositories.Interfaces;

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

        return services;
    }
}