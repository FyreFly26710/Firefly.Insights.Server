using System;
using MassTransit.Mediator;

namespace Server.Messages.Identities;

public record AgentListRequestMessage() : Request<AgentListRequestMessageResponse>;

public record AgentListRequestMessageResponse(List<UserTo> UserTos);