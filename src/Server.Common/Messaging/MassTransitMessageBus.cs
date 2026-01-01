using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Common.Messaging;

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
        CancellationToken cancellationToken = default,
        TimeSpan? timeout = null)
        where TRequest : class
        where TResponse : class
    {
        var requestClient = _serviceProvider.GetRequiredService<IRequestClient<TRequest>>();
        var timeoutValue = timeout ?? TimeSpan.FromSeconds(30);
        var response = await requestClient.GetResponse<TResponse>(request, cancellationToken, RequestTimeout.After(s: (int)timeoutValue.TotalSeconds));
        return response.Message;
    }
}