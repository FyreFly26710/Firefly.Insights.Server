namespace Server.Contents.Api.Application.Commands;

public record TopicUpdateCommand(TopicUpdateRequest Request) : IRequest<bool>;
public class TopicUpdateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<TopicUpdateCommand, bool>
{
    public async Task<bool> Handle(TopicUpdateCommand command, CancellationToken cancellationToken)
    {
        var topic = await _contentsContext.Topics.FindAsync(command.Request.TopicId, cancellationToken);
        if (topic is null)
            throw new ExceptionNotFound($"Topic of id {command.Request.TopicId} not found");

        if (command.Request.CategoryId is not null)
        {
            var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
            if (category is null)
                throw new ExceptionNotFound($"Category of id {command.Request.CategoryId} not found");
        }

        if (command.Request.TopicArticles is not null)
        {
            // remove all existing topic articles
            var existingTopicArticles = await _contentsContext.ArticleMetas.Where(x => x.TopicId == topic.Id).ToListAsync(cancellationToken);
            foreach (var existingTopicArticle in existingTopicArticles)
            {
                existingTopicArticle.TopicId = -1L;
                existingTopicArticle.UpdatedAt = DateTime.UtcNow;
            }

            // Add new topic articles and update article meta
            var articleIds = command.Request.TopicArticles.Select(x => x.ArticleId).ToList();
            var articleQuery = _contentsContext.Articles.AsQueryable().Include(x => x.ArticleMeta);
            var articles = await articleQuery.Where(x => articleIds.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var article in articles)
            {
                var topicArticle = command.Request.TopicArticles.FirstOrDefault(x => x.ArticleId == article.Id);
                if (topicArticle is not null)
                {
                    // article.Title = topicArticle.Title;
                    // article.UpdatedAt = DateTime.UtcNow;
                    article.ArticleMeta.IsTopicSummary = topicArticle.IsTopicSummary;
                    article.ArticleMeta.SortNumber = topicArticle.SortNumber;
                    article.ArticleMeta.IsHidden = topicArticle.IsHidden;
                    article.ArticleMeta.UpdatedAt = DateTime.UtcNow;
                    article.ArticleMeta.TopicId = topic.Id;
                }
            }

        }

        topic.Name = command.Request.Name ?? topic.Name;
        topic.Description = command.Request.Description ?? topic.Description;
        topic.CategoryId = command.Request.CategoryId ?? topic.CategoryId;
        topic.ImageUrl = command.Request.ImageUrl ?? topic.ImageUrl;
        topic.SortNumber = command.Request.SortNumber ?? topic.SortNumber;
        topic.IsHidden = command.Request.IsHidden ?? topic.IsHidden;
        topic.UpdatedAt = DateTime.UtcNow;

        await _contentsContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
public class TopicUpdateRequestValidator : AbstractValidator<TopicUpdateRequest>
{
    public TopicUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Topic name is required.")
            .When(x => x.Name is not null);

    }
}