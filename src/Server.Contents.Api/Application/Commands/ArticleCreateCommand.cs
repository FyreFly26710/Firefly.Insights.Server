using System;

namespace Server.Contents.Api.Application.Commands;

public record ArticleCreateCommand(ArticleCreateRequest Request) : IRequest<long?>;
public class ArticleCreateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<ArticleCreateCommand, long?>
{
    public async Task<long?> Handle(ArticleCreateCommand command, CancellationToken cancellationToken)
    {
        var topicId = command.Request.TopicId;
        var topic = await _contentsContext.Topics.FindAsync(command.Request.TopicId, cancellationToken);
        if (topic is null)
            throw new ExceptionNotFound($"Topic of id {command.Request.TopicId} not found");

        var article = new Article()
        {
            Title = command.Request.Title,
            Content = command.Request.Content,
            Description = command.Request.Description,
            ArticleMeta = new ArticleMeta()
            {
                TopicId = topicId,
                IsTopicSummary = command.Request.IsTopicSummary,
                ImageUrl = command.Request.ImageUrl,
                UserId = 1,
                SortNumber = command.Request.SortNumber,
                IsHidden = command.Request.IsHidden,
            }
        };
        await _contentsContext.Articles.AddAsync(article, cancellationToken);
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return article.Id;
    }
}

public class ArticleCreateRequestValidator : AbstractValidator<ArticleCreateRequest>
{
    public ArticleCreateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(128).WithMessage("Title cannot exceed 128 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(256).WithMessage("Description cannot exceed 256 characters.");
        RuleFor(x => x.ImageUrl)
            .MaximumLength(256).WithMessage("Image URL cannot exceed 256 characters.")
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Image URL must be a valid URL.");
        RuleFor(x => x.TopicId)
            .NotEmpty().WithMessage("Topic ID is required.");
    }
}