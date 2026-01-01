using System;
using MassTransit.Mediator;

namespace Server.Messages.Identities;

public record UserRequestMessage(long UserId) : Request<UserRequestMessageResponse>;

public record UserRequestMessageResponse(UserTo UserTo);