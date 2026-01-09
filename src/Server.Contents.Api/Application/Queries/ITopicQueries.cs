namespace Server.Contents.Api.Application.Queries;

public interface ITopicQueries
{
    Task<TopicDto> GetTopicById(long topicId);
    Task<Paged<TopicDto>> GetTopicList(TopicListRequest request);
    Task<List<LookupItemDto>> GetLookupList();
    Task<long> GetSummaryArticleId(long topicId);
}
