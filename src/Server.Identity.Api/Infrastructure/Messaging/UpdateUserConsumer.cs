using System;
using Server.Common.Types;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class UpdateUserConsumer(UserContext _userContext) : IConsumer<UpdateUserMessage>
{
    public async Task Consume(ConsumeContext<UpdateUserMessage> context)
    {
        var message = context.Message;
        var user = await _userContext.Users.FindAsync(message.UserId);
        if (user is null)
            throw new ExceptionNotFound($"User of id {message.UserId} not found");
            
        user.UserName = message.UserName ?? user.UserName;
        user.UserAvatar = message.UserAvatar ?? user.UserAvatar;
        user.UserRole = message.UserRole ?? user.UserRole;
        user.UpdatedAt = DateTime.UtcNow;
        await _userContext.SaveChangesAsync();
    }

}
