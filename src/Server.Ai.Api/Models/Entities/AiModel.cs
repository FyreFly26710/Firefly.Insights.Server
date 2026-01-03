using System;

namespace Server.Ai.Api.Models.Entities;

public class AiModel : Entity
{
    public string Provider { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public decimal InputPrice { get; set; } = 0;
    public decimal OutputPrice { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    public string ApiKey { get; set; } = string.Empty;

}

public static class AiModelsSeed
{
    public static List<AiModel> GetAiModels(IConfiguration configuration)
    {
        var openAiApiKey = configuration.GetValue<string>("OpenAi:ApiKey") ?? string.Empty;
        var geminiApiKey = configuration.GetValue<string>("Gemini:ApiKey") ?? string.Empty;
        return [
            new() { Provider = "OpenAI", Model = "gpt-5.2", ModelId = "gpt-5.2-2025-12-11", InputPrice = 1.75M, OutputPrice = 14.00M, ApiKey = openAiApiKey },
            new() { Provider = "OpenAI", Model = "gpt-5", ModelId = "gpt-5-2025-08-07", InputPrice = 1.25M, OutputPrice = 10.00M, ApiKey = openAiApiKey },
            new() { Provider = "OpenAI", Model = "gpt-5-mini", ModelId = "gpt-5-mini-2025-08-07", InputPrice = 0.25M, OutputPrice = 2.00M, ApiKey = openAiApiKey },
            new() { Provider = "OpenAI", Model = "gpt-5-nano", ModelId = "gpt-5-nano-2025-08-07", InputPrice = 0.05M, OutputPrice = 0.40M, ApiKey = openAiApiKey },
            new() { Provider = "OpenAI", Model = "gpt-4.1", ModelId = "gpt-4.1-2025-04-14", InputPrice = 2.00M, OutputPrice = 8.00M, ApiKey = openAiApiKey },
            new() { Provider = "Gemini", Model = "gemini-3-pro", ModelId = "gemini-3-pro-preview", InputPrice = 2M, OutputPrice = 12M, ApiKey = geminiApiKey },
            new() { Provider = "Gemini", Model = "gemini-3-flash", ModelId = "gemini-3-flash-preview", InputPrice = 0.5M, OutputPrice = 3M, ApiKey = geminiApiKey },
            new() { Provider = "Gemini", Model = "gemini-2.5-pro", ModelId = "gemini-2.5-pro", InputPrice = 1.25M, OutputPrice = 10M, ApiKey = geminiApiKey },
            new() { Provider = "Gemini", Model = "gemini-2.5-flash", ModelId = "gemini-2.5-flash", InputPrice = 0.3M, OutputPrice = 2.5M, ApiKey = geminiApiKey },
            new() { Provider = "Gemini", Model = "gemini-2.5-flash-lite", ModelId = "gemini-2.5-flash-lite", InputPrice = 0.1M, OutputPrice = 0.4M, ApiKey = geminiApiKey },
        ];
    }
}