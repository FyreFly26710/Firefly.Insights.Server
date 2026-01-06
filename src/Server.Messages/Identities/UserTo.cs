using System;

namespace Server.Messages.Identities;

public record UserTo(long UserId, string UserName = "", string UserAvatar = "", string UserRole = "", string UserAccount = "");