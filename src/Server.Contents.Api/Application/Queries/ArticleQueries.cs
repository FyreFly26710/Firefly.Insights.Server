using Server.Common.Extensions;

namespace Server.Contents.Api.Application.Queries;

public class ArticleQueries(ContentsContext _contentsContext, ILogger<ArticleQueries> _logger) : IArticleQueries
{
    private IQueryable<ArticleMeta> GetArticleNavigationQuery()
    {
        return _contentsContext.ArticleMetas.AsQueryable().AsNoTracking()
            .Include(am => am.Article)
            .Include(am => am.ArticleTags).ThenInclude(at => at.Tag)
            .Include(am => am.Topic);
    }
    public async Task<ArticleDto> GetArticleById(long articleId)
    {
        var query = GetArticleNavigationQuery();

        var articleMeta = await query.FirstOrDefaultAsync(am => am.ArticleId == articleId);
        if (articleMeta is null)
            throw new ExceptionNotFound();

        return articleMeta.Article.ToArticleDto("Unknown");
    }

    public async Task<Paged<ArticleDto>> GetArticleList(ArticleListRequest request)
    {
        PageInfo pagedInfo = request;

        var query = GetArticleNavigationQuery();

        if (!string.IsNullOrEmpty(request.ArticleTitle))
        {
            // The pattern % surrounds the search term to mimic .Contains() for case insensitive search
            var pattern = $"%{request.ArticleTitle}%";
            query = query.Where(am => EF.Functions.ILike(am.Article.Title, pattern));
        }
        if (request.TopicId is not null)
            query = query.Where(am => am.TopicId == request.TopicId);
        if (request.IsTopicSummary is not null)
            query = query.Where(am => am.IsTopicSummary == request.IsTopicSummary);
        if (request.IsHidden is not null)
            query = query.Where(am => am.IsHidden == request.IsHidden);
        if (request.UserId is not null)
            query = query.Where(am => am.UserId == request.UserId);
        if (request.Tags is not null)
            query = query.Where(am => am.ArticleTags.Any(at => request.Tags.Contains(at.Tag.Name)));

        var pagedData = await query.ToPagedDtoAsync(pagedInfo, am => am.Article.ToArticleDto("Unknown"));
        return pagedData;
    }
}
