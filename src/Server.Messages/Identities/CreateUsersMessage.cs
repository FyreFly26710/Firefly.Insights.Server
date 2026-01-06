using System;

namespace Server.Messages.Identities;

public record CreateUsersMessage(List<UserTo> Users);