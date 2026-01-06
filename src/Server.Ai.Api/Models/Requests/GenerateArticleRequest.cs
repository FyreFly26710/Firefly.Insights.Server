using System;

namespace Server.Ai.Api.Models.Requests;

public record GenerateArticleSummaryRequest(
    long AiModelId,
    long UserId,
    string UserPrompt,

    int ArticleCount,
    long CategoryId,
    string Category,
    string Topic,
    string TopicDescription,
    string TopicUrl
    );