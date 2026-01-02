using System;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class GetAgentRequestConsumer(UserContext _userContext) : IConsumer<GetAgentRequestMessage>
{
    public async Task Consume(ConsumeContext<GetAgentRequestMessage> context)
    {
        var message = context.Message;
        var agent = await _userContext.Users.FirstOrDefaultAsync(u => u.UserAccount == message.AgentModelName);
        if (agent is not null)
        {
            await context.RespondAsync(new GetAgentRequestMessageResponse(new UserTo(agent.Id, agent.UserName, agent.UserAvatar, agent.UserRole)));
        }
        else
        {
            await context.RespondAsync(new GetAgentRequestMessageResponse(new UserTo(0, "Unknown", "", "agent")));
        }
    }

}
