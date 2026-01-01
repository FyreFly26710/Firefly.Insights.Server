using System;

namespace Server.Ai.Api.Models.Requests;

public record GenerateArticleSummaryRequest(
    AiAgent Agent,
    long UserId,
    string UserPrompt,

    int ArticleCount,
    string Category,
    string Topic,
    string TopicDescription
    );