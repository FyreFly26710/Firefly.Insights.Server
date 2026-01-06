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
            AiProviderId = request.AiProviderId,
            Model = request.Model,
            Avatar = request.Avatar,
            ModelId = request.ModelId,
            InputPrice = request.InputPrice,
            OutputPrice = request.OutputPrice,
            IsActive = request.IsActive,
            DisplayName = request.DisplayName,
            Description = request.Description,
        };
        _aiContext.AiModels.Add(model);
        await _aiContext.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new CreateUsersMessage(new List<UserTo> { new UserTo(model.Id, model.DisplayName, model.Avatar, "agent", model.Id.ToString()) }), cancellationToken);
        return true;
    }

}

public class CreateAiModelRequestValidator : AbstractValidator<CreateAiModelRequest>
{
    public CreateAiModelRequestValidator()
    {
        RuleFor(x => x.AiProviderId).GreaterThan(0).WithMessage("AI provider ID is required.");
        RuleFor(x => x.Model).MaximumLength(128).NotEmpty().WithMessage("Model is required.");
        RuleFor(x => x.ModelId).MaximumLength(128).NotEmpty().WithMessage("Model ID is required.");
        // RuleFor(x => x.InputPrice).GreaterThan(0).WithMessage("Input price is required.");
        // RuleFor(x => x.OutputPrice).GreaterThan(0).WithMessage("Output price is required.");
        // RuleFor(x => x.IsActive).IsInEnum().WithMessage("Is active is required.");
        RuleFor(x => x.DisplayName).MaximumLength(128).NotEmpty().WithMessage("Display name is required.");
        RuleFor(x => x.Avatar).MaximumLength(1024);
        RuleFor(x => x.Description).MaximumLength(1024);
    }
}