using MediatR;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Application.Commands;
public record RegisterUserCommand(string UserAccount, string UserPassword) : IRequest<bool>;
