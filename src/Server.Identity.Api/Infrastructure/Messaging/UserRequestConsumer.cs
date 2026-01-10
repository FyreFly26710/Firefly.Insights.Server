namespace Server.Identity.Api.Infrastructure.Messaging;

public class UserRequestConsumer(UserContext _userContext, ILogger<UserRequestConsumer> _logger) : IConsumer<UserRequestMessage>
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
            _logger.LogError("User of id {UserId} not found", message.UserId);
            var userTo = new UserTo(message.UserId, string.Empty, string.Empty, string.Empty);
            await context.RespondAsync(new UserRequestMessageResponse(userTo));
        }
    }
}
