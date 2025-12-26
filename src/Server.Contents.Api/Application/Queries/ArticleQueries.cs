using Server.Common.Extensions;
using Server.Contents.Api.Infrastructure.EfContexts;

namespace Server.Contents.Api.Application.Queries;

public class ArticleQueries(ContentsContext _contentsContext, ILogger<ArticleQueries> _logger) : IArticleQueries
{
    private IQueryable<Article> GetArticleNavigationQuery()
    {
        return _contentsContext.Articles.AsQueryable().AsNoTracking()
            .Include(a => a.ArticleMeta).ThenInclude(am => am.ArticleTags).ThenInclude(at => at.Tag)
            .Include(a => a.ArticleMeta).ThenInclude(am => am.Topic);
    }
    public async Task<ArticleDto> GetArticleById(long articleId)
    {
        var query = GetArticleNavigationQuery();

        var article = await query.FirstOrDefaultAsync(a => a.Id == articleId);
        if (article is null)
            throw new ExceptionNotFound();

        return article.ToArticleDto("Unknown");
    }

    public async Task<Paged<ArticleDto>> GetArticleList(ArticleListRequest request)
    {
        PageInfo pagedInfo = request;

        var query = GetArticleNavigationQuery();

        if (!string.IsNullOrEmpty(request.ArticleTitle))
            query = query.Where(a => a.Title.Contains(request.ArticleTitle));

        var pagedData = await query.ToPagedDtoAsync(pagedInfo, a => a.ToArticleDto("Unknown"));
        return pagedData;
    }
}
