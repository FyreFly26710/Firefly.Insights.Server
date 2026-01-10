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
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Publishing message. MessageName: {MessageName}", typeof(T).Name);
        return _publishEndpoint.Publish(message, cancellationToken);
    }
    public async Task<TResponse> RequestAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default,
        TimeSpan? timeout = null)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogInformation("Requesting message. RequestName: {RequestName}", typeof(TRequest).Name);
        var requestClient = _serviceProvider.GetRequiredService<IRequestClient<TRequest>>();
        var timeoutValue = timeout ?? TimeSpan.FromMinutes(1);
        var response = await requestClient.GetResponse<TResponse>(request, cancellationToken, RequestTimeout.After(s: (int)timeoutValue.TotalSeconds));
        _logger.LogInformation("Response received. ResponseName: {ResponseName}", typeof(TResponse).Name);
        return response.Message;
    }
}