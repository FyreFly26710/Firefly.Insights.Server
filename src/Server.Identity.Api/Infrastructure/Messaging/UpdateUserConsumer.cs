using System;
using Server.Common.Types;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class UpdateUserConsumer(UserContext _userContext, ILogger<UpdateUserConsumer> _logger) : IConsumer<UpdateUserMessage>
{
    public async Task Consume(ConsumeContext<UpdateUserMessage> context)
    {
        var message = context.Message;
        var user = await _userContext.Users.FindAsync(message.UserId);
        if (user is null)
        {
            _logger.LogError("User of id {UserId} not found", message.UserId);
            return;
        }

        user.UserName = message.UserName ?? user.UserName;
        user.UserAvatar = message.UserAvatar ?? user.UserAvatar;
        user.UserRole = message.UserRole ?? user.UserRole;
        user.UpdatedAt = DateTime.UtcNow;
        await _userContext.SaveChangesAsync();
    }

}
