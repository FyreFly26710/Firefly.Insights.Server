using Server.Common.Extensions;
using Server.Contents.Api.Infrastructure.EfContexts;
using Server.Contents.Api.Infrastructure.RedisRepositories;

namespace Server.Contents.Api.Application.Queries;

public class ArticleQueries(ContentsContext _contentsContext, ITagRepository _tagRepository, ILogger<ArticleQueries> _logger) : IArticleQueries
{
    private IQueryable<Article> GetArticleNavigationQuery()
    {
        return _contentsContext.Articles.AsQueryable().AsNoTracking()
            .Include(a => a.ArticleMeta).ThenInclude(am => am.ArticleTags)
            .Include(a => a.ArticleMeta).ThenInclude(am => am.Topic);
    }
    public async Task<ArticleDto> GetArticleById(long articleId)
    {
        var query = GetArticleNavigationQuery();

        var article = await query.FirstOrDefaultAsync(a => a.Id == articleId);
        if (article is null)
            throw new ExceptionNotFound();
        var tags = await _tagRepository.GetTagsByIdsAsync(article.ArticleMeta.ArticleTags.Select(at => at.TagId));
        var articleDto = article.ToArticleDto("Unknown", tags.Select(t => t.ToTagDto()).ToList());
        return articleDto;
    }

    public async Task<Paged<ArticleDto>> GetArticleList(ArticleListRequest request)
    {
        PageInfo pagedInfo = request;

        var query = GetArticleNavigationQuery();

        if (!string.IsNullOrEmpty(request.ArticleTitle))
            query = query.Where(a => a.Title.Contains(request.ArticleTitle));
        // get paged entities
        var pagedData = await query.ToPagedAsync(pagedInfo);
        // get article tags
        var articleTagsDict = pagedData.Data.ToDictionary(a => a.Id, a => a.ArticleMeta.ArticleTags.Select(at => at.TagId).ToList());
        var tags = await _tagRepository.GetTagsByIdsAsync(articleTagsDict.Values.SelectMany(t => t).Distinct().ToList());

        // convert to dtos
        var dtos = pagedData.Data.Select(a => a.ToArticleDto()).ToList();
        foreach (var dto in dtos)
        {
            dto.Tags = tags.Where(t => articleTagsDict[dto.ArticleId].Contains(t.Id)).Select(t => t.ToTagDto()).ToList();
        }
        return new Paged<ArticleDto>(pagedInfo, pagedData.TotalCount, dtos);
    }
}
