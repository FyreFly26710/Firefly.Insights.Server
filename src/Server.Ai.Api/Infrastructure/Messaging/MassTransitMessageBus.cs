using System;
using MassTransit;
using Server.Ai.Api.Application.Services;

namespace Server.Ai.Api.Infrastructure.Messaging;

public class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IServiceProvider _serviceProvider;
    public MassTransitMessageBus(IPublishEndpoint publishEndpoint, IServiceProvider serviceProvider)
    {
        _publishEndpoint = publishEndpoint;
        _serviceProvider = serviceProvider;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        return _publishEndpoint.Publish(message, cancellationToken);
    }
    public async Task<TResponse> RequestAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : class
        where TResponse : class
    {
        var requestClient = _serviceProvider.GetRequiredService<IRequestClient<TRequest>>();
        var response = await requestClient.GetResponse<TResponse>(request, cancellationToken);
        return response.Message;
    }
}