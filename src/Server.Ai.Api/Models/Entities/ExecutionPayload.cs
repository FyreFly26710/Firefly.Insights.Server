using System;

namespace Server.Ai.Api.Models.Entities;

public class ExecutionPayload : Entity
{
    public string RequestJson { get; set; } = string.Empty;
    public string? ResponseJson { get; set; }

}
