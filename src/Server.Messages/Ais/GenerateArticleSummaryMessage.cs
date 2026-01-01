using System;

namespace Server.Messages.Ais;

public record GenerateArticleSummaryMessage(
    long JobId,
    long UserId,
    long AiModelId,
    string Prompt,
    int ArticleCount,
    string Topic,
    string TopicDescription,
    string Category
);