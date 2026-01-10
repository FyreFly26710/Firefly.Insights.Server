using Server.Messages.Ais;

namespace Server.Ai.Api.Application.Commands;

public record GenerateArticleSummaryCommand(GenerateArticleSummaryRequest Request) : IRequest<bool>;

// Validate the request and create a job log
public class GenerateArticleSummaryCommandHandler(ILogger<GenerateArticleSummaryCommandHandler> _logger,
    IArticleGenerationClient _articleGenerationClient,
    AiContext _aiContext,
    IMessageBus _messageBus)
    : IRequestHandler<GenerateArticleSummaryCommand, bool>
{
    public async Task<bool> Handle(GenerateArticleSummaryCommand command, CancellationToken cancellationToken)
    {
        var userId = 1;
        var request = command.Request;
        var model = await _aiContext.AiModels.Include(x => x.AiProvider).FirstOrDefaultAsync(x => x.Id == request.AiModelId, cancellationToken);
        if (model is null)
            throw new ExceptionNotFound($"AI model of id {request.AiModelId} not found");

        var job = new JobLog
        {
            UserId = userId,
            JobType = AiJobType.Articles_Summary,
            Status = AiGenerationJobStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            AiModelId = model.Id,
        };

        _aiContext.JobLogs.Add(job);
        await _aiContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Initializing article summary job. JobLogId: {JobLogId}", job.Id);
        var message = new GenerateArticleSummaryMessage(
            job.Id,
            userId,
            // model.Id,
            request.UserPrompt,
            request.ArticleCount,
            request.Topic,
            request.TopicDescription,
            request.Category,
            request.CategoryId,
            request.TopicUrl);
        await _messageBus.PublishAsync(message, cancellationToken);
        return true;
    }
}


public class GenerateArticleSummaryCommandValidator : AbstractValidator<GenerateArticleSummaryCommand>
{
    public GenerateArticleSummaryCommandValidator()
    {
        // RuleFor(x => x.Request.Provider).NotEmpty();
        // RuleFor(x => x.Request.Model).NotEmpty();
        // RuleFor(x => x.Request.UserId).NotEmpty();
        RuleFor(x => x.Request.ArticleCount).GreaterThan(0).LessThanOrEqualTo(30);
        RuleFor(x => x.Request.Topic).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Request.TopicDescription).NotEmpty().MaximumLength(512);
    }
}