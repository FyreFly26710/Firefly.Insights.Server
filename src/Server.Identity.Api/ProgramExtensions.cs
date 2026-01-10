using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Server.Common.Behaviours;
using Server.Common.Extensions;
using Server.Common.Messaging;
using Server.Common.Utils;
using Server.Identity.Api.Application.Queries;
using Server.Identity.Api.Application.Services;
using Server.Identity.Api.Infrastructure.Messaging;
using Server.Identity.Api.Infrastructure.Services;

namespace Server.Identity.Api;
public static class ProgramExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        services.AddFluentValidationAutoValidation();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarker));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        services.AddScoped<IUserQueries, UserQueries>();

        return services;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOAuthService, OAuthService>();
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IMessageBus, MassTransitMessageBus>();

        services.AddMassTransit(x =>
        {
            // Add consumers
            x.AddConsumer<UserRequestConsumer>();
            x.AddConsumer<UserListRequestConsumer>();
            x.AddConsumer<AgentListRequestConsumer>();
            x.AddConsumer<CreateUsersConsumer>();
            x.AddConsumer<UpdateUserConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"] ?? "localhost", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

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

