using System;
using GeminiDotnet;
using GeminiDotnet.Extensions.AI;
using Microsoft.Extensions.AI;
using Server.Ai.DeepSeek;
using Server.Messages.Ais;
using Server.Messages.Contents;

namespace Server.Ai.Api.Infrastructure.AiClients;

public class ArticleGenerationClient
    (ILogger<ArticleGenerationClient> _logger, AiContext _aiContext)
    : IArticleGenerationClient
{
    public async Task<string> GenerateArticleContentAsync(long jobLogId, long aiModelId, GenerationArticleSummary articleSummary, TopicTo topic, CancellationToken cancellationToken = default)
    {
        List<string> tags = [articleSummary.SkillLevelTag, articleSummary.FocusAreaTag, articleSummary.ArticleStyleTag, articleSummary.TechStackTag, articleSummary.ToneTag];
        var prompt = Prompts.System_ArticleContent(topic.CategoryName, topic.Name, topic.Description, articleSummary.Title, articleSummary.Description, tags);
        List<ChatMessage> messages = [new(ChatRole.User, prompt)];

        var response = await ExecuteAiAgentAsync(jobLogId, aiModelId, prompt, messages, new ChatOptions(), cancellationToken);
        return response;
    }
    public async Task<string> GenerateArticleSummaryListAsync(long jobLogId, long aiModelId, int articleCount, string topic, string topicDescription, string category, string userPrompt, CancellationToken cancellationToken = default)
    {
        ChatOptions chatOptions = new() { ResponseFormat = ChatResponseFormat.ForJsonSchema<GenerationArticleList>() };

        var fullPrompt = Prompts.System_ArticleList(articleCount, topic, topicDescription, category, userPrompt);
        List<ChatMessage> messages = [new(ChatRole.User, fullPrompt)];

        var response = await ExecuteAiAgentAsync(jobLogId, aiModelId, fullPrompt, messages, chatOptions, cancellationToken);
        return response;

    }
    public async Task<string> GenerateTopicSummaryAsync(long jobLogId, long aiModelId, TopicTo topic, CancellationToken cancellationToken = default)
    {
        if (topic.TopicArticles == null || !topic.TopicArticles.Any())
            return "No articles available for this topic.";

        var prompt = Prompts.System_TopicSummary(topic.CategoryName, topic.Name, topic.Description, topic.TopicId, topic.TopicArticles);
        List<ChatMessage> messages = [new(ChatRole.User, prompt)];
        var response = await ExecuteAiAgentAsync(jobLogId, aiModelId, prompt, messages, new ChatOptions(), cancellationToken);
        return response;
    }

    private async Task<string> ExecuteAiAgentAsync(long jobLogId, long aiModelId, string prompt, List<ChatMessage> messages, ChatOptions chatOptions, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _aiContext.AiModels.Include(x => x.AiProvider).FirstOrDefaultAsync(x => x.Id == aiModelId && x.IsActive, cancellationToken);
            if (model == null)
                throw new ExceptionNotFound($"AiModel with id {aiModelId} not found or is not active");
            IChatClient chatClient = GetChatClient(model.AiProvider.Name, model.ModelId, model.AiProvider.ApiKey);

            chatOptions.ModelId = model.ModelId;
            chatOptions.MaxOutputTokens = 8192;

            var startTime = DateTime.UtcNow;
            var response = await chatClient.GetResponseAsync(messages, chatOptions, cancellationToken);
            var endTime = DateTime.UtcNow;

            ExecutionLog executionLog = CreateExecutionLog(jobLogId, model, prompt, startTime, response, endTime);

            _aiContext.ExecutionLogs.Add(executionLog);
            await _aiContext.SaveChangesAsync(cancellationToken);
            return executionLog.ExecutionPayload!.Response!;
        }
        catch (Exception ex)
        {

            var executionLog = new ExecutionLog(jobLogId, DateTime.UtcNow, ex.Message, prompt);
            _aiContext.ExecutionLogs.Add(executionLog);
            await _aiContext.SaveChangesAsync(cancellationToken);
            throw;
        }
    }

    private static ExecutionLog CreateExecutionLog(long jobLogId, AiModel model, string prompt, DateTime startTime, ChatResponse response, DateTime endTime)
    {
        var reasoningTokens = response.Usage?.ReasoningTokenCount ?? 0; // Reasoning tokens are included in the output tokens
        var inputTokens = response.Usage?.InputTokenCount ?? 0;
        var outputTokens = response.Usage?.OutputTokenCount ?? 0;
        var cost = (inputTokens * model.InputPrice + outputTokens * model.OutputPrice) / 1000000;
        var duration = endTime - startTime;

        string responseMessage = response.Messages.Last().Text;
        ExecutionLog executionLog = new()
        {
            JobLogId = jobLogId,
            ExecutedAt = startTime,
            InputTokens = (int)inputTokens,
            OutputTokens = (int)outputTokens,
            ReasoningTokens = (int)reasoningTokens,
            Cost = cost,
            Duration = duration,
            IsSuccessful = true,
            ExecutionPayload = new ExecutionPayload(prompt, responseMessage)
        };
        return executionLog;
    }

    private IChatClient GetChatClient(string provider, string modelId, string apiKey)
    {
        return provider switch
        {
            "Gemini" => new GeminiChatClient(new GeminiClientOptions
            {
                ApiKey = apiKey,
                ModelId = modelId
            }),
            "OpenAI" => new OpenAI.Chat.ChatClient(modelId, apiKey).AsIChatClient(),
            "DeepSeek" => new DeepSeekChatClient(apiKey),
            _ => throw new NotSupportedException($"Provider {provider} is not implemented"),
        };
    }
}