using System;
using GeminiDotnet;
using GeminiDotnet.Extensions.AI;
using Microsoft.Extensions.AI;
using Server.Messages.Ais;

namespace Server.Ai.Api.Infrastructure.AiClients;

public class ArticleGenerationClient(ILogger<ArticleGenerationClient> _logger, IConfiguration _configuration, AiContext _aiContext) : IArticleGenerationClient
{
    public async Task GenerateArticleSummaryListAsync(GenerateArticleSummaryMessage message, CancellationToken cancellationToken = default)
    {
        var model = _aiContext.AiModels.Find(message.AiModelId);

        IChatClient chatClient = GetChatClient(model.Provider, model.ModelId);
        ChatOptions chatOptions = new() { ResponseFormat = ChatResponseFormat.ForJsonSchema<GenerationArticleList>() };
        var prompt = Prompts.System_ArticleList(message.ArticleCount, message.Topic, message.TopicDescription, message.Category, message.Prompt);
        List<ChatMessage> messages = [new(ChatRole.User, prompt)];

        try
        {
            var startTime = DateTime.UtcNow;
            var response = await chatClient.GetResponseAsync(messages, chatOptions, cancellationToken);
            var endTime = DateTime.UtcNow;

            var reasoningTokens = response.Usage?.ReasoningTokenCount ?? 0; // Reasoning tokens are included in the output tokens
            var inputTokens = response.Usage?.InputTokenCount ?? 0;
            var outputTokens = response.Usage?.OutputTokenCount ?? 0;
            var cost = (inputTokens * model.InputPrice + outputTokens * model.OutputPrice) / 1000000;
            var duration = endTime - startTime;

            var executionLog = new ExecutionLog
            {
                JobLogId = message.JobId,
                ExecutedAt = startTime,
                InputTokens = (int)inputTokens,
                OutputTokens = (int)outputTokens,
                ReasoningTokens = (int)reasoningTokens,
                Cost = cost,
                Duration = duration,
                IsSuccessful = true,
                ExecutionPayload = new ExecutionPayload(prompt, response.Messages.Last().Text)
            };

            _aiContext.ExecutionLogs.Add(executionLog);
            await _aiContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {

            var executionLog = new ExecutionLog(message.JobId, DateTime.UtcNow, ex.Message);
            _aiContext.ExecutionLogs.Add(executionLog);
            await _aiContext.SaveChangesAsync(cancellationToken);
            throw;
        }

    }

    private IChatClient GetChatClient(string provider, string modelId)
    {
        return provider switch
        {
            "Gemini" => new GeminiChatClient(new GeminiClientOptions
            {
                ApiKey = _configuration.GetValue<string>("Gemini:ApiKey") ?? throw new InvalidOperationException("Gemini:ApiKey is not set"),
                ModelId = modelId
            }),
            "OpenAI" => new OpenAI.Chat.ChatClient(modelId, _configuration.GetValue<string>("OpenAi:ApiKey")).AsIChatClient(),
            _ => throw new NotSupportedException($"Provider {provider} is not implemented"),
        };
    }
}