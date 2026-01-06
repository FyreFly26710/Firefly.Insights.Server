using System;
using MassTransit.Mediator;
using Server.Messages.Identities;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class AgentListRequestConsumer(UserContext _userContext) : IConsumer<AgentListRequestMessage>
{
    public async Task Consume(ConsumeContext<AgentListRequestMessage> context)
    {
        var message = context.Message;
        var users = await _userContext.Users.Where(u => u.UserRole == "agent").ToListAsync(context.CancellationToken);
        var userTos = users.Select(u => u.ToUserTo()).ToList();
        await context.RespondAsync(new AgentListRequestMessageResponse(userTos));
    }

}
