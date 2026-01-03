using System;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class DeleteAgentConsumer(UserContext _userContext) : IConsumer<DeleteAgentMessage>
{
    public async Task Consume(ConsumeContext<DeleteAgentMessage> context)
    {
        var message = context.Message;
        var user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserAccount == message.Model);
        if (user is not null)
        {
            await _userContext.SaveChangesAsync();
        }
    }

}
