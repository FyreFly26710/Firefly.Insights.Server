using System;

namespace Server.Ai.Api.Models.Types;

public record AiAgent(
    AiAgentProvider Provider,
    string Model
);
