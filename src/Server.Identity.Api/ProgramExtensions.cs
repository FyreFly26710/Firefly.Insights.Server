using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Server.Identity.Api.Application.Queries;
using Server.Identity.Api.Application.Services;
using Server.Identity.Api.Infrastructure;
namespace Server.Identity.Api;
public static class ProgramExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddValidatorsFromAssemblyContaining<IIdentityAssemblyMarker>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(IIdentityAssemblyMarker));
        });

        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<UserContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("UserDb"));
        });

        return services;
    }


}

