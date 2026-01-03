using System;

namespace Server.Messages.Identities;

public record CreateAgentMessage(string UserName, string UserAccount, string UserAvatar);