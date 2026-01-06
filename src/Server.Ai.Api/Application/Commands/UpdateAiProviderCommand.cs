using System;

namespace Server.Ai.Api.Application.Commands;

public record UpdateAiProviderCommand(UpdateAiProviderRequest Request, long AiProviderId) : IRequest<bool>;
public class UpdateAiProviderCommandHandler(AiContext _aiContext, IMessageBus _messageBus) : IRequestHandler<UpdateAiProviderCommand, bool>
{
    public async Task<bool> Handle(UpdateAiProviderCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var provider = await _aiContext.AiProviders.FindAsync(command.AiProviderId, cancellationToken);
        if (provider is null)
            throw new ExceptionNotFound($"AI provider of id {command.AiProviderId} not found");

        provider.ApiKey = request.ApiKey ?? provider.ApiKey;
        await _aiContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}