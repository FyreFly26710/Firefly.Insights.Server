using Microsoft.EntityFrameworkCore;
using Server.Common.Types;
using Server.Contents.Api.Infrastructure;
using Server.Contents.Api.Models.Entities;
using Server.Contents.Api.Models.Responses;

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
    public async Task<List<TopicDto>> GetTopicList()
    {
        var query = GetNavigationQuery(false);

        var topics = await query.ToListAsync();
        
        topics = topics.OrderBy(t => t.SortNumber).ToList();
        return topics.Select(t => t.ToTopicDto()).ToList();
    }
}
