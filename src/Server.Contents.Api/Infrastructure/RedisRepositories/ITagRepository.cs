using System;

namespace Server.Contents.Api.Infrastructure.RedisRepositories;

public interface ITagRepository
{
    Task<Tag> GetOrAddTagAsync(string name, TagType type);
    Task<List<Tag>> GetTagsByIdsAsync(IEnumerable<long> ids);
    void AddRange(IEnumerable<Tag> tags);
}
