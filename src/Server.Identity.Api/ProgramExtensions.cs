using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Server.Common.Utils;
using Server.Identity.Api.Application.Queries;
using Server.Identity.Api.Application.Services;
using Server.Identity.Api.Infrastructure;
namespace Server.Identity.Api;
public static class ProgramExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddValidatorsFromAssemblyContaining<IIdentityAssemblyMarker>();
        services.AddFluentValidationAutoValidation();

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
        var connectionString = configuration.GetConnectionString("UserDb");
        services.AddDbContext<UserContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        if (EnvUtil.IsDevelopment())
        {
            services.AddMigration<UserContext, UserContextSeed>();
        }

        return services;
    }


}

