namespace Server.Identity.Api.Infrastructure.Messaging;

public class UserRequestConsumer(UserContext _userContext) : IConsumer<UserRequestMessage>
{
    public async Task Consume(ConsumeContext<UserRequestMessage> context)
    {
        var message = context.Message;
        var user = await _userContext.Users.FindAsync(message.UserId);
        if (user != null)
        {
            var userTo = new UserTo(message.UserId, user.UserName, user.UserAvatar, user.UserRole);
            await context.RespondAsync(new UserRequestMessageResponse(userTo));
        }
        else
        {
            var userTo = new UserTo(message.UserId, string.Empty, string.Empty, string.Empty);
            await context.RespondAsync(new UserRequestMessageResponse(userTo));
        }
    }
}
