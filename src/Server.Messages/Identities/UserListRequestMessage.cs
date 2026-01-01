using System;
using MassTransit.Mediator;

namespace Server.Messages.Identities;

public record UserListRequestMessage(List<long> UserIds) : Request<UserListRequestMessageResponse>;

public record UserListRequestMessageResponse(List<UserTo> UserTos);