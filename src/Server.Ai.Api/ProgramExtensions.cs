using FluentValidation;
using FluentValidation.AspNetCore;
using Server.Ai.Api.Infrastructure.AiClients;
using Server.Ai.Api.Infrastructure.Messaging;

namespace Server.Ai.Api;
public static class ProgramExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        services.AddFluentValidationAutoValidation();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarker));
        });


        return services;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAiClient, AiClient>();
        // services.AddScoped<IMessageBus, MassTransitMessageBus>();

        // services.AddMassTransit(x =>
        // {
        //     // Add consumers
        //     x.AddConsumer<GenerateArticleSummaryConsumer>();

        //     x.UsingRabbitMq((context, cfg) =>
        //     {
        //         cfg.Host(configuration["RabbitMq:Host"], h =>
        //         {
        //             h.Username(configuration["RabbitMq:Username"] ?? "guest");
        //             h.Password(configuration["RabbitMq:Password"] ?? "guest");
        //         });
        //         // global retry policy for all consumers
        //         cfg.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromMinutes(10)); });
        //         cfg.ConfigureEndpoints(context);
        //     });
        // });

        return services;
    }


}

