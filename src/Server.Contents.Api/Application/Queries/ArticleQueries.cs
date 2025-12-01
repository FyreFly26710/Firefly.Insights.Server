using System;
using Microsoft.EntityFrameworkCore;
using Server.Common.Extensions;
using Server.Common.Types;
using Server.Contents.Api.Infrastructure;
using Server.Contents.Api.Models.Entities;
using Server.Contents.Api.Models.Requests;
using Server.Contents.Api.Models.Responses;

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

        return article.ToArticleDto();
    }

    public async Task<Paged<ArticleDto>> GetArticleList(ArticleListRequest request)
    {
        PageRequest pagedInfo = request;

        var query = GetArticleNavigationQuery();

        if (!string.IsNullOrEmpty(request.ArticleTitle))
            query = query.Where(a => a.Title.Contains(request.ArticleTitle));

        var pagedData = await query.ToPagedAsync(pagedInfo);
        var dtos = pagedData.Data.Select(a => a.ToArticleDto()).ToList();
        return new Paged<ArticleDto>(pagedInfo, pagedData.TotalCount, dtos);
    }
}
