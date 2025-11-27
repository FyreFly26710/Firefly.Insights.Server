using FluentValidation;
using Server.Identity.Api.Contracts;
using Server.Identity.Api.Interfaces.Services;
using Server.Identity.Api.Services;

namespace Server.Identity.Api;
public static class ProgramExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddApplications(configuration);


        return services;
    }

    static IServiceCollection AddApplications(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<IIdentityAssemblyMarker>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

}

