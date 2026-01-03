using System;

namespace Server.Messages.Identities;

public record UpdateAgentMessage(string Model, string? UserName, string? UserAvatar);