namespace Server.Ai.Api.Application.Commands;

public record GenerateArticleSummaryCommand(GenerateArticleSummaryRequest Request) : IRequest<bool>;
public class GenerateArticleSummaryCommandHandler(ILogger<GenerateArticleSummaryCommandHandler> _logger)
    // IMessageBus _messageBus)
    : IRequestHandler<GenerateArticleSummaryCommand, bool>
{
    public async Task<bool> Handle(GenerateArticleSummaryCommand command, CancellationToken cancellationToken)
    {
        // await _messageBus.PublishAsync(command.Request, cancellationToken);
        return true;
    }
}
