namespace Server.Ai.Api.Application.Commands;

public record UpdateAiModelCommand(UpdateAiModelRequest Request, long AiModelId) : IRequest<bool>;
public class UpdateAiModelCommandHandler(AiContext _aiContext, IMessageBus _messageBus) : IRequestHandler<UpdateAiModelCommand, bool>
{
    public async Task<bool> Handle(UpdateAiModelCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var model = await _aiContext.AiModels.FindAsync(command.AiModelId, cancellationToken);
        if (model is null)
            throw new ExceptionNotFound($"AI model of id {command.AiModelId} not found");
        model.Provider = request.Provider ?? model.Provider;
        model.Model = request.Model ?? model.Model;
        model.ModelId = request.ModelId ?? model.ModelId;
        model.InputPrice = request.InputPrice ?? model.InputPrice;
        model.OutputPrice = request.OutputPrice ?? model.OutputPrice;
        model.IsActive = request.IsActive ?? model.IsActive;
        model.ApiKey = request.ApiKey ?? model.ApiKey;
        await _aiContext.SaveChangesAsync(cancellationToken);

        if (request.AgentName is not null || request.AgentAvatarUrl is not null)
        {
            await _messageBus.PublishAsync(new UpdateAgentMessage(model.Model, request.AgentName, request.AgentAvatarUrl), cancellationToken);
        }
        return true;
    }
}
public class UpdateAiModelRequestValidator : AbstractValidator<UpdateAiModelRequest>
{
    public UpdateAiModelRequestValidator()
    {
        RuleFor(x => x.Provider).MaximumLength(128);
        RuleFor(x => x.Model).MaximumLength(128);
        RuleFor(x => x.ModelId).MaximumLength(128);
        RuleFor(x => x.ApiKey).MaximumLength(1024);
        RuleFor(x => x.AgentName).MaximumLength(128);
        RuleFor(x => x.AgentAvatarUrl).MaximumLength(256);

    }
}