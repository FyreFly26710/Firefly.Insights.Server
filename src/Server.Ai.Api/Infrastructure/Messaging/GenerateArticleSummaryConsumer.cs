using System;

namespace Server.Ai.Api.Infrastructure.Messaging;

public class GenerateArticleSummaryConsumer(ILogger<GenerateArticleSummaryConsumer> _logger, IAiClient _aiClient)
    : IConsumer<GenerateArticleSummaryRequest>
{
    public async Task Consume(ConsumeContext<GenerateArticleSummaryRequest> context)
    {
        var message = context.Message;
        var result = await _aiClient.GenerateArticleSummaryList(message);
        _logger.LogInformation("Generated article summary list: {Result}", result);
    }
}