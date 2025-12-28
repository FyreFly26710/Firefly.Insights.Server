namespace Server.Contents.Api.Application.Commands;

public record ArticleUpdateCommand(ArticleUpdateRequest Request) : IRequest<bool>;
public class ArticleUpdateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<ArticleUpdateCommand, bool>
{
    public async Task<bool> Handle(ArticleUpdateCommand command, CancellationToken cancellationToken)
    {
        var article = await _contentsContext.Articles
            .Include(a => a.ArticleMeta)
            .FirstOrDefaultAsync(a => a.Id == command.Request.ArticleId, cancellationToken);
        if (article is null)
            throw new ExceptionNotFound($"Article of id {command.Request.ArticleId} not found");


        if (command.Request.TopicId is not null)
        {
            var topic = await _contentsContext.Topics.FindAsync(command.Request.TopicId, cancellationToken);
            if (topic is null)
                throw new ExceptionNotFound($"Topic of id {command.Request.TopicId} not found");
            article.ArticleMeta.TopicId = topic.Id;
        }

        article.Title = command.Request.Title ?? article.Title;
        article.Content = command.Request.Content ?? article.Content;
        article.Description = command.Request.Description ?? article.Description;
        article.ArticleMeta.ImageUrl = command.Request.ImageUrl ?? article.ArticleMeta.ImageUrl;
        article.ArticleMeta.IsTopicSummary = command.Request.IsTopicSummary ?? article.ArticleMeta.IsTopicSummary;
        article.ArticleMeta.SortNumber = command.Request.SortNumber ?? article.ArticleMeta.SortNumber;
        article.ArticleMeta.IsHidden = command.Request.IsHidden ?? article.ArticleMeta.IsHidden;
        article.ArticleMeta.UpdatedAt = DateTime.UtcNow;
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class ArticleUpdateRequestValidator : AbstractValidator<ArticleUpdateRequest>
{
    public ArticleUpdateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(128).WithMessage("Title cannot exceed 128 characters.")
            .When(x => x.Title is not null);
        RuleFor(x => x.Description)
            .MaximumLength(256).WithMessage("Description cannot exceed 256 characters.");
        RuleFor(x => x.ImageUrl)
            .MaximumLength(256).WithMessage("Image URL cannot exceed 256 characters.")
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Image URL must be a valid URL.");
    }
}