namespace Server.Identity.Api.Infrastructure.Messaging;

public class UserListRequestConsumer(UserContext _userContext) : IConsumer<UserListRequestMessage>
{
    public async Task Consume(ConsumeContext<UserListRequestMessage> context)
    {
        var message = context.Message;
        var userIds = message.UserIds;
        var users = await _userContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync(context.CancellationToken);
        var userTos = users.Select(u => new UserTo(u.Id, u.UserName, u.UserAvatar, u.UserRole)).ToList();
        await context.RespondAsync(new UserListRequestMessageResponse(userTos));
    }
}
