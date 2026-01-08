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

        // model.Provider = request.Provider ?? model.Provider;
        model.AiProviderId = request.AiProviderId ?? model.AiProviderId;
        model.Model = request.Model ?? model.Model;
        model.ModelId = request.ModelId ?? model.ModelId;
        model.InputPrice = request.InputPrice ?? model.InputPrice;
        model.OutputPrice = request.OutputPrice ?? model.OutputPrice;
        model.IsActive = request.IsActive ?? model.IsActive;
        model.DisplayName = request.DisplayName ?? model.DisplayName;
        model.Avatar = request.Avatar ?? model.Avatar;
        model.Description = request.Description ?? model.Description;
        // model.ApiKey = request.ApiKey ?? model.ApiKey;
        await _aiContext.SaveChangesAsync(cancellationToken);

        if (request.DisplayName is not null || request.Avatar is not null)
        {
            await _messageBus.PublishAsync(new UpdateUserMessage(model.Id, request.DisplayName, request.Avatar), cancellationToken);
        }
        return true;
    }
}
public class UpdateAiModelRequestValidator : AbstractValidator<UpdateAiModelRequest>
{
    public UpdateAiModelRequestValidator()
    {
        // RuleFor(x => x.AiProviderId).GreaterThan(0).WithMessage("AI provider ID is required.");
        RuleFor(x => x.Model).MaximumLength(128);
        RuleFor(x => x.ModelId).MaximumLength(128);
        // RuleFor(x => x.InputPrice).GreaterThan(0).WithMessage("Input price is required.");
        // RuleFor(x => x.OutputPrice).GreaterThan(0).WithMessage("Output price is required.");
        // RuleFor(x => x.IsActive).IsInEnum().WithMessage("Is active is required.");
        RuleFor(x => x.DisplayName).MaximumLength(128);
        RuleFor(x => x.Avatar).MaximumLength(1024);
        RuleFor(x => x.Description).MaximumLength(1024);
    }
}