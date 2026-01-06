using System;

namespace Server.Ai.Api.Models.Entities;

public class AiModel : AuditableEntity
{
    public long AiProviderId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public decimal InputPrice { get; set; } = 0M;
    public decimal OutputPrice { get; set; } = 0M;
    public bool IsActive { get; set; } = true;

    public AiProvider AiProvider { get; set; } = new();
}

public static class AiModelsSeed
{
    public static List<AiModel> GetAiModels(IConfiguration configuration)
    {
        var openAiApiKey = configuration.GetValue<string>("OpenAi:ApiKey") ?? string.Empty;
        var geminiApiKey = configuration.GetValue<string>("Gemini:ApiKey") ?? string.Empty;
        var openAiProvider = new AiProvider { Name = "OpenAI", ApiKey = openAiApiKey };
        var geminiProvider = new AiProvider { Name = "Gemini", ApiKey = geminiApiKey };
        string AVATAR_GPT_V1 = "https://static.vecteezy.com/system/resources/previews/021/608/790/non_2x/chatgpt-logo-chat-gpt-icon-on-black-background-free-vector.jpg";
        string AVATAR_GPT_V2 = "https://research.aimultiple.com/wp-content/uploads/2023/03/chatgpt.webp";
        string AVATAR_GEMINI_V1 = "https://registry.npmmirror.com/@lobehub/icons-static-png/latest/files/dark/gemini-color.png";
        string AVATAR_GEMINI_V2 = "https://images.seeklogo.com/logo-png/62/1/google-gemini-icon-logo-png_seeklogo-623016.png";

        return [
            new() { AiProvider = openAiProvider, Model = "gpt-5.2", ModelId = "gpt-5.2-2025-12-11", InputPrice = 1.75M, OutputPrice = 14.00M, DisplayName = "GPT-5.2", Avatar = AVATAR_GPT_V2 },
            new() { AiProvider = openAiProvider, Model = "gpt-5", ModelId = "gpt-5-2025-08-07", InputPrice = 1.25M, OutputPrice = 10.00M, DisplayName = "GPT-5", Avatar = AVATAR_GPT_V2 },
            new() { AiProvider = openAiProvider, Model = "gpt-5-mini", ModelId = "gpt-5-mini-2025-08-07", InputPrice = 0.25M, OutputPrice = 2.00M, DisplayName = "GPT-5 Mini", Avatar = AVATAR_GPT_V1 },
            new() { AiProvider = openAiProvider, Model = "gpt-5-nano", ModelId = "gpt-5-nano-2025-08-07", InputPrice = 0.05M, OutputPrice = 0.40M, DisplayName = "GPT-5 Nano", Avatar = AVATAR_GPT_V1 },
            new() { AiProvider = openAiProvider, Model = "gpt-4.1", ModelId = "gpt-4.1-2025-04-14", InputPrice = 2.00M, OutputPrice = 8.00M, DisplayName = "GPT-4.1", Avatar = AVATAR_GPT_V1 },
            new() { AiProvider = geminiProvider, Model = "gemini-3-pro", ModelId = "gemini-3-pro-preview", InputPrice = 2M, OutputPrice = 12M, DisplayName = "Gemini 3 Pro", Avatar = AVATAR_GEMINI_V2 },
            new() { AiProvider = geminiProvider, Model = "gemini-3-flash", ModelId = "gemini-3-flash-preview", InputPrice = 0.5M, OutputPrice = 3M, DisplayName = "Gemini 3 Flash", Avatar = AVATAR_GEMINI_V1 },
            new() { AiProvider = geminiProvider, Model = "gemini-2.5-pro", ModelId = "gemini-2.5-pro", InputPrice = 1.25M, OutputPrice = 10M, DisplayName = "Gemini 2.5 Pro", Avatar = AVATAR_GEMINI_V2 },
            new() { AiProvider = geminiProvider, Model = "gemini-2.5-flash", ModelId = "gemini-2.5-flash", InputPrice = 0.3M, OutputPrice = 2.5M, DisplayName = "Gemini 2.5 Flash", Avatar = AVATAR_GEMINI_V1 },
            new() { AiProvider = geminiProvider, Model = "gemini-2.5-flash-lite", ModelId = "gemini-2.5-flash-lite", InputPrice = 0.1M, OutputPrice = 0.4M, DisplayName = "Gemini 2.5 Flash Lite", Avatar = AVATAR_GEMINI_V1 },
        ];
    }
}