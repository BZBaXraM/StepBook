using AuthMiddleware.Jwt;

namespace StepBook.API.Gateway.Extensions;

public static class DiExtension
{
    public static IServiceCollection AuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterJwt(configuration);
        services.AddSingleton<JwtMiddleware>();

        return services;
    }
}