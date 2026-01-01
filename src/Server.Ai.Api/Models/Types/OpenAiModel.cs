using System;

namespace Server.Ai.Api.Models.Types;
/// <summary>
/// https://platform.openai.com/docs/models
/// </summary>
public class OpenAiModel
{
    public const string Gpt52 = "gpt-5.2-2025-12-11"; //The best model for coding and agentic tasks across industries
    public const string Gpt5Mini = "gpt-5-mini-2025-08-07"; //A faster, cost-efficient version of GPT-5 for well-defined tasks
    public const string Gpt5Nano = "gpt-5-nano-2025-08-07"; //Fastest, most cost-efficient version of GPT-5
    public const string Gpt41 = "gpt-4.1-2025-04-14"; //Smartest non-reasoning model

    public static HashSet<string> SupportedModels = [
        Gpt52,
        Gpt5Mini,
        Gpt5Nano,
        Gpt41,
    ];
}
