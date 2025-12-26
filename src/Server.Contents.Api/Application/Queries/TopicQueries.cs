using Server.Contents.Api.Infrastructure.EfContexts;
using Server.Contents.Api.Infrastructure.RedisRepositories;

namespace Server.Contents.Api.Application.Queries;

public class TopicQueries(ContentsContext _contentsContext, ITagRepository _tagRepository, ILogger<TopicQueries> _logger) : ITopicQueries
{
    private IQueryable<Topic> GetNavigationQuery(bool withArticles)
    {
        IQueryable<Topic> query = _contentsContext.Topics.AsQueryable().AsNoTracking()
            .Include(a => a.Category);
        if (withArticles)
        {
            query = query.Include(t => t.ArticleMetas).ThenInclude(am => am.Article)
                         .Include(a => a.ArticleMetas).ThenInclude(am => am.ArticleTags);
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
        var topicDto = topic.ToTopicDto();
        var tagIds = topicDto.TopicArticles?.SelectMany(a => a.TagIds).Distinct().ToList() ?? [];
        var tags = await _tagRepository.GetTagsByIdsAsync(tagIds);
        foreach (var article in topicDto.TopicArticles ?? [])
        {
            article.Tags = tags.Where(t => article.TagIds.Contains(t.Id)).Select(t => t.ToTagDto()).ToList();
        }
        return topicDto;
    }
    public async Task<List<TopicDto>> GetTopicList()
    {
        var query = GetNavigationQuery(false);

        var topics = await query.ToListAsync();
        
        topics = topics.OrderBy(t => t.SortNumber).ToList();
        return topics.Select(t => t.ToTopicDto()).ToList();
    }
}
