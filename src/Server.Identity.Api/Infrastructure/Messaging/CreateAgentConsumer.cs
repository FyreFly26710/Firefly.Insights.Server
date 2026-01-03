using System;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class CreateAgentConsumer(UserContext _userContext) : IConsumer<CreateAgentMessage>
{
    public async Task Consume(ConsumeContext<CreateAgentMessage> context)
    {
        var message = context.Message;
        var user = new User()
        {
            UserName = message.UserName,
            UserAccount = message.UserAccount,
            UserPassword = "Password",
            UserAvatar = message.UserAvatar,
            UserRole = "agent"
        };
        _userContext.Users.Add(user);
        await _userContext.SaveChangesAsync();

    }

}
