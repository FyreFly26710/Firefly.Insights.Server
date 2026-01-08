namespace Server.Contents.Api.Application.Commands;

public record ArticleCreateCommand(ArticleCreateRequest Request, long UserId) : IRequest<long?>;
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
                UserId = command.UserId,
                SortNumber = command.Request.SortNumber,
                IsHidden = command.Request.IsHidden,
            }
        };

        if (command.Request.Tags is not null && command.Request.Tags.Count > 0)
        {
            var tags = await _contentsContext.UpsertTagsAsync(command.Request.Tags);
            article.ArticleMeta.ArticleTags = tags.Select(t => new ArticleTag() { ArticleMetaId = article.ArticleMeta.Id, TagId = t.Id }).ToList();
        }

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
            .NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.TopicId)
            .NotEmpty().WithMessage("Topic ID is required.");
    }
}