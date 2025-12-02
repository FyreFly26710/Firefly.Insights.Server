using Server.Common.Types;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Application.Queries;

public interface ITopicQueries
{
    Task<TopicDto> GetTopicById(long topicId);
    Task<List<TopicDto>> GetTopicList();
}
