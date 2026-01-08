using System;
using Server.Identity.Api.Models.Constants;

namespace Server.Identity.Api.Infrastructure.Messaging;

public class CreateUsersConsumer(UserContext _userContext) : IConsumer<CreateUsersMessage>
{
    public async Task Consume(ConsumeContext<CreateUsersMessage> context)
    {
        var message = context.Message;
        var incomingUsers = message.Users;

        var incomingIds = incomingUsers.Select(u => u.UserId).ToList();

        var existingIds = await _userContext.Users
            .Where(u => incomingIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync();

        var existingIdsSet = new HashSet<long>(existingIds);

        var newUsers = incomingUsers
            .Where(u => !existingIdsSet.Contains(u.UserId))
            .Select(u => new User()
            {
                Id = u.UserId,
                UserName = u.UserName,
                UserAccount = u.UserAccount,
                UserPassword = Passwords.DefaultPassword, 
                UserAvatar = u.UserAvatar,
                UserRole = u.UserRole
            })
            .ToList();

        // 4. Add and Save only if there are new users
        if (newUsers.Any())
        {
            await _userContext.Users.AddRangeAsync(newUsers);
            await _userContext.SaveChangesAsync();
        }
    }

}
