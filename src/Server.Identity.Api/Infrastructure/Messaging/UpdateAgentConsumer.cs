using System;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class UpdateAgentConsumer(UserContext _userContext) : IConsumer<UpdateAgentMessage>
{
    public async Task Consume(ConsumeContext<UpdateAgentMessage> context)
    {
        var message = context.Message;
        var user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserAccount == message.Model);
        if (user is not null)
        {
            user.UserName = message.UserName ?? user.UserName;
            user.UserAvatar = message.UserAvatar ?? user.UserAvatar;
            await _userContext.SaveChangesAsync();
        }
    }
}
