using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Server.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private const int MaxLogLength = 800; 

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var requestJson = SerializeAndTrim(request);
        _logger.LogInformation("Handling command {CommandName}: {RequestJson}", requestName, requestJson);

        var response = await next();

        var responseJson = SerializeAndTrim(response);
        _logger.LogInformation("Command {CommandName} handled. Response: {ResponseJson}", requestName, responseJson);

        return response;
    }

    private static string SerializeAndTrim(object? value)
    {
        if (value == null) return "null";

        try
        {
            string json = JsonSerializer.Serialize(value);

            if (json.Length <= MaxLogLength)
                return json;

            // Trim the middle
            int keepSide = (MaxLogLength - 5) / 2; // 5 for the "[...]"
            return string.Concat(
                json.AsSpan(0, keepSide),
                "[...]",
                json.AsSpan(json.Length - keepSide)
            );
        }
        catch (Exception ex)
        {
            return $"[Serialization Error: {ex.Message}]";
        }
    }
}