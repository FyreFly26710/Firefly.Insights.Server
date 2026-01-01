using System;

namespace Server.Ai.Api.Models.Types;
/// <summary>
/// https://ai.google.dev/gemini-api/docs/models#gemini-3-pro
/// </summary>
public class GeminiModel
{
    public const string Gemini3ProPreview = "gemini-3-pro-preview";
    public const string Gemini3FlashPreview = "gemini-3-flash-preview";
    public const string Gemini25Flash = "gemini-2.5-flash";

    public static HashSet<string> SupportedModels = [
        Gemini3ProPreview,
        Gemini3FlashPreview,
        Gemini25Flash
    ];
}
