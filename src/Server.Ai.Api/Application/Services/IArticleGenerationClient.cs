using System;
using Server.Messages.Ais;
using Server.Messages.Contents;

namespace Server.Ai.Api.Application.Services;

public interface IArticleGenerationClient
{
    Task<string> GenerateArticleSummaryListAsync(long jobLogId, long aiModelId, int articleCount, string topic, string topicDescription, string category, string userPrompt, CancellationToken cancellationToken = default);
    Task<string> GenerateArticleContentAsync(long jobLogId, long aiModelId, GenerationArticleSummary articleSummary, TopicTo topic, CancellationToken cancellationToken = default);
    Task<string> GenerateTopicSummaryAsync(long jobLogId, long aiModelId, TopicTo topic, CancellationToken cancellationToken = default);
}
