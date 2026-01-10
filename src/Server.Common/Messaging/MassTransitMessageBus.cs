using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server.Common.Messaging;

public class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MassTransitMessageBus> _logger;
    public MassTransitMessageBus(IPublishEndpoint publishEndpoint, IServiceProvider serviceProvider, ILogger<MassTransitMessageBus> logger)
    {
        _publishEndpoint = publishEndpoint;
        _serviceProvider = serviceProvider;
        _logger = logger;
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
        var timeoutValue = timeout ?? TimeSpan.FromMinutes(1);
        var response = await requestClient.GetResponse<TResponse>(request, cancellationToken, RequestTimeout.After(s: (int)timeoutValue.TotalSeconds));
        return response.Message;
    }
}