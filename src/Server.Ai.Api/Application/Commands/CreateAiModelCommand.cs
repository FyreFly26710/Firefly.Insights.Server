using System;

namespace Server.Ai.Api.Application.Commands;

public record CreateAiModelCommand(CreateAiModelRequest Request) : IRequest<bool>;
public class CreateAiModelCommandHandler(AiContext _aiContext, IMessageBus _messageBus) : IRequestHandler<CreateAiModelCommand, bool>
{
    public async Task<bool> Handle(CreateAiModelCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var model = new AiModel
        {
            Provider = request.Provider,
            Model = request.Model,
            ModelId = request.ModelId,
            InputPrice = request.InputPrice,
            OutputPrice = request.OutputPrice,
            IsActive = request.IsActive,
            ApiKey = request.ApiKey,
        };
        _aiContext.AiModels.Add(model);
        await _aiContext.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new CreateAgentMessage(request.AgentName, request.Model, request.AgentAvatarUrl), cancellationToken);
        return true;
    }

}

public class CreateAiModelRequestValidator : AbstractValidator<CreateAiModelRequest>
{
    public CreateAiModelRequestValidator()
    {
        RuleFor(x => x.Provider).MaximumLength(128).NotEmpty().WithMessage("Provider is required.");
        RuleFor(x => x.Model).MaximumLength(128).NotEmpty().WithMessage("Model is required.");
        RuleFor(x => x.ModelId).MaximumLength(128).NotEmpty().WithMessage("Model ID is required.");
        RuleFor(x => x.ApiKey).MaximumLength(1024);
        RuleFor(x => x.AgentName).MaximumLength(128);
        RuleFor(x => x.AgentAvatarUrl).MaximumLength(256);
    }
}