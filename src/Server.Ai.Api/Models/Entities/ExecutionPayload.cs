using System;

namespace Server.Ai.Api.Models.Entities;

public class ExecutionPayload : Entity
{
    public ExecutionPayload(){}
    public ExecutionPayload(string prompt, string? response)
    {
        Prompt = prompt;
        Response = response;
    }
    public string Prompt { get; set; } = string.Empty;
    public string? Response { get; set; }

}
