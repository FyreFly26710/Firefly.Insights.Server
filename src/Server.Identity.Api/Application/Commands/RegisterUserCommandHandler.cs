using MediatR;
using Server.Identity.Api.Infrastructure;
using Server.Identity.Api.Models.Entities;

namespace Server.Identity.Api.Application.Commands;
public class RegisterUserCommandHandler(UserContext _userContext)
: IRequestHandler<RegisterUserCommand, bool>
{
    public async Task<bool> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            UserAccount = command.UserAccount,
            UserPassword = command.UserPassword,
            UserRole = "admin",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _userContext.Users.AddAsync(user, cancellationToken);
        await _userContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
