using System;

namespace Server.Ai.Api.Models.Requests;

public record GenerateArticleSummaryRequest(
    string Provider,
    string Model,
    long UserId,
    string UserPrompt,

    int ArticleCount,
    // Assume the category is already created in the database
    long CategoryId, 
    string Category, 
    string Topic,
    string TopicDescription
    );