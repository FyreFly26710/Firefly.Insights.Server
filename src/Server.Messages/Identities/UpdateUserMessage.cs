using System;

namespace Server.Messages.Identities;

public record UpdateUserMessage(long UserId, string? UserName = null, string? UserAvatar = null, string? UserRole = null);