using System;

namespace Server.Ai.Api.Infrastructure.Messaging;

public class GenerateArticleSummaryConsumer : IConsumer<GenerateArticleSummaryRequest>
{
    public async Task Consume(ConsumeContext<GenerateArticleSummaryRequest> context)
    {
        var message = context.Message;
    }
}