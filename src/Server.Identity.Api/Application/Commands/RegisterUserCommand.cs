using MediatR;

namespace Server.Identity.Api.Application.Commands;
public record RegisterUserCommand(string UserAccount, string UserPassword) : IRequest<bool>;
