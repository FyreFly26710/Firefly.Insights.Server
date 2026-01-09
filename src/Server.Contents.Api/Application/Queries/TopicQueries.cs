using Server.Common.Extensions;

namespace Server.Contents.Api.Application.Queries;

public class TopicQueries(ContentsContext _contentsContext, ILogger<TopicQueries> _logger) : ITopicQueries
{
    private IQueryable<Topic> GetNavigationQuery(bool withArticles)
    {
        IQueryable<Topic> query = _contentsContext.Topics.AsQueryable().AsNoTracking()
            .Include(a => a.Category);
        if (withArticles)
        {
            query = query.Include(t => t.ArticleMetas).ThenInclude(am => am.Article)
                         .Include(a => a.ArticleMetas).ThenInclude(am => am.ArticleTags).ThenInclude(at => at.Tag);
        }
        return query;
    }
    public async Task<TopicDto> GetTopicById(long topicId)
    {
        var query = GetNavigationQuery(true);

        var topic = await query.FirstOrDefaultAsync(t => t.Id == topicId);
        if (topic is null)
            throw new ExceptionNotFound();

        topic.ArticleMetas = topic.ArticleMetas.OrderBy(am => am.SortNumber).ToList();
        return topic.ToTopicDto();
    }
    public async Task<Paged<TopicDto>> GetTopicList(TopicListRequest request)
    {
        PageInfo pagedInfo = request;

        var query = GetNavigationQuery(false);

        if (!string.IsNullOrEmpty(request.TopicName))
        {
            // The pattern % surrounds the search term to mimic .Contains() for case insensitive search
            var pattern = $"%{request.TopicName}%";
            query = query.Where(t => EF.Functions.ILike(t.Name, pattern));
        }
        if (request.CategoryId is not null)
            query = query.Where(t => t.CategoryId == request.CategoryId);
        if (request.IsHidden is not null)
            query = query.Where(t => t.IsHidden == request.IsHidden);

        var pagedData = await query.ToPagedDtoAsync(pagedInfo, t => t.ToTopicDto());
        return pagedData;
    }
    public async Task<List<LookupItemDto>> GetLookupList()
    {
        var query = _contentsContext.Topics.AsQueryable().AsNoTracking();
        var topics = await query.Select(t => new LookupItemDto(t.Id, t.Name)).ToListAsync();
        return topics;
    }

    public async Task<long> GetSummaryArticleId(long topicId)
    {
        var query = GetNavigationQuery(true);
        var topic = await query.FirstOrDefaultAsync(t => t.Id == topicId);
        if (topic is null)
            throw new ExceptionNotFound();
        // if no articles are found, return 0
        if (!topic.ArticleMetas.Any())
            return 0;

        // return the summary article if found
        var summaryArticle = topic.ArticleMetas.FirstOrDefault(am => am.IsTopicSummary);
        if (summaryArticle is not null)
            return summaryArticle.ArticleId;

        // return the first article
        return topic.ArticleMetas.OrderBy(am => am.SortNumber).First().ArticleId;
    }
}
