using System;
using GeminiDotnet;
using GeminiDotnet.Extensions.AI;
using Microsoft.Extensions.AI;
using Server.Messages.Ais;

namespace Server.Ai.Api.Infrastructure.AiClients;

public class AiClient : IAiClient
{
    private IConfiguration _configuration;
    public AiClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> GenerateArticleSummaryList(GenerateArticleSummaryRequest request)
    {
        var modelId = request.Agent.Model.ToLower();
        if (request.Agent.Provider == AiAgentProvider.Gemini)
        {
            if (!GeminiModel.SupportedModels.Contains(modelId))
                throw new NotSupportedException($"Model {request.Agent.Model} is not supported for Gemini");
        }
        else if (request.Agent.Provider == AiAgentProvider.OpenAI)
        {
            if (!OpenAiModel.SupportedModels.Contains(modelId))
                throw new NotSupportedException($"Model {request.Agent.Model} is not supported for OpenAI");
        }

        IChatClient chatClient = request.Agent.Provider switch
        {
            AiAgentProvider.Gemini => new GeminiChatClient(new GeminiClientOptions
            {
                ApiKey = _configuration.GetValue<string>("Gemini:ApiKey") ?? throw new InvalidOperationException("Gemini:ApiKey is not set"),
                ModelId = modelId
            }),
            AiAgentProvider.OpenAI => new OpenAI.Chat.ChatClient(modelId, _configuration.GetValue<string>("OpenAi:ApiKey")).AsIChatClient(),
            _ => throw new NotSupportedException($"Provider {request.Agent.Provider} is not supported"),
        };


        ChatOptions chatOptions = new()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<GenerationArticleList>()
        };

        List<ChatMessage> messages = new();
        messages.Add(new(ChatRole.User, Prompts.System_ArticleList(request.ArticleCount, request.Topic, request.TopicDescription, request.Category)));

        if (!string.IsNullOrWhiteSpace(request.UserPrompt))
            messages.Add(new ChatMessage(ChatRole.User, request.UserPrompt));
            
        var response = await chatClient.GetResponseAsync(messages, chatOptions);
        return response.Messages.Last().Text;
    }
}