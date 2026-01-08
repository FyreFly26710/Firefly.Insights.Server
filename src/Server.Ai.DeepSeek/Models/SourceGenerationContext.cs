using System.Text.Json.Serialization;

namespace Server.Ai.DeepSeek.Models;


[JsonSerializable(typeof(ChatRequest))]
[JsonSerializable(typeof(ChatResponse))]
[JsonSerializable(typeof(CompletionRequest))]
[JsonSerializable(typeof(Message))]
[JsonSerializable(typeof(ModelResponse))]
[JsonSerializable(typeof(UserResponse))]
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class SourceGenerationContext : JsonSerializerContext
{
}
