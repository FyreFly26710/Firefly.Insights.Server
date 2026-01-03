using System;

namespace Server.Ai.Api.Application.Commands;

public record DeleteAiModelCommand(long AiModelId) : IRequest<bool>;
public class DeleteAiModelCommandHandler(AiContext _aiContext, IMessageBus _messageBus) : IRequestHandler<DeleteAiModelCommand, bool>
{
    public async Task<bool> Handle(DeleteAiModelCommand command, CancellationToken cancellationToken)
    {
        var model = await _aiContext.AiModels.FindAsync(command.AiModelId, cancellationToken);
        if (model is null)
            return true;
            
        _aiContext.AiModels.Remove(model);
        await _aiContext.SaveChangesAsync(cancellationToken);
        await _messageBus.PublishAsync(new DeleteAgentMessage(model.Model), cancellationToken);
        return true;
    }

}
