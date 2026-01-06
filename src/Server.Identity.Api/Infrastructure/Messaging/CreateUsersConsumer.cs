using System;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class CreateUsersConsumer(UserContext _userContext) : IConsumer<CreateUsersMessage>
{
    public async Task Consume(ConsumeContext<CreateUsersMessage> context)
    {
        var message = context.Message;
        var users = message.Users;
        await _userContext.Users.AddRangeAsync(users.Select(u => new User()
        {
            Id = u.UserId,
            UserName = u.UserName,
            UserAccount = u.UserAccount,
            UserPassword = "Password",
            UserAvatar = u.UserAvatar,
            UserRole = u.UserRole
        }));
        await _userContext.SaveChangesAsync();
    }

}
