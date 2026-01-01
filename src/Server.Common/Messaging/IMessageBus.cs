using System;

namespace Server.Common.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;

    Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
        where TRequest : class
        where TResponse : class;
}

// Register the message bus in the DI container
/*
    services.AddScoped<IMessageBus, MassTransitMessageBus>();
    services.AddMassTransit(x =>
    {
        // Add consumers
        // x.AddConsumer<TConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(configuration["RabbitMq:Host"], h =>
            {
                h.Username(configuration["RabbitMq:Username"] ?? "guest");
                h.Password(configuration["RabbitMq:Password"] ?? "guest");
            });
            // cfg.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromMinutes(10)); });
            cfg.ConfigureEndpoints(context);
        });
    });
*/