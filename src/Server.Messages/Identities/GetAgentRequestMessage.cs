using System;
using MassTransit.Mediator;

namespace Server.Messages.Identities;

public record GetAgentRequestMessage(string AgentModelName) : Request<GetAgentRequestMessageResponse>;

public record GetAgentRequestMessageResponse(UserTo UserTo);