namespace Server.Contents.Api.Application.Commands;

public record ArticleDeleteCommand(long ArticleId) : IRequest<bool>;
public class ArticleDeleteCommandHandler(ContentsContext _contentsContext) : IRequestHandler<ArticleDeleteCommand, bool>
{
    public async Task<bool> Handle(ArticleDeleteCommand command, CancellationToken cancellationToken)
    {
        var article = await _contentsContext.Articles
            .Include(a => a.ArticleMeta)
            .ThenInclude(am => am.ArticleTags)
            .FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken);
        if (article is null)
            throw new ExceptionNotFound($"Article of id {command.ArticleId} not found");
        article.IsDeleted = true;
        article.UpdatedAt = DateTime.UtcNow;
        article.ArticleMeta.IsDeleted = true;
        article.ArticleMeta.UpdatedAt = DateTime.UtcNow;
        foreach (var articleTag in article.ArticleMeta.ArticleTags)
        {
            articleTag.IsDeleted = true;
            articleTag.UpdatedAt = DateTime.UtcNow;
        }
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}