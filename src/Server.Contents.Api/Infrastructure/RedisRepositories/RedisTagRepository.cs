using System.Text.Json;
using StackExchange.Redis;

namespace Server.Contents.Api.Infrastructure.RedisRepositories;

/// <summary>
/// Redis data storage for tags:
///
/// <b>Keys for tag names per type:</b>
///    For each TagType, there is a Redis hash mapping tag names to tag IDs:
///        Key: "tags:name:SkillLevel"
///        Value (Hash):
///            "Unallocated" => 0
///            "Beginner"    => 1
///            "Advanced"    => 2
///            ...
///        Key pattern: tags:name:{TagType}
///        Hash field = tag name
///        Hash value = tag ID
///    This allows fast lookup of a tag by name and type.
/// </summary>
public class RedisTagRepository : ITagRepository
{
    private readonly IDatabase _redis;
    public RedisTagRepository(IConnectionMultiplexer connection) => _redis = connection.GetDatabase();

    public async Task<Tag> GetOrAddTagAsync(string name, TagType type)
    {
        string typeKey = $"tags:name:{type}";

        var idValue = await _redis.HashGetAsync(typeKey, name);
        if (idValue.HasValue)
            return new Tag { Id = (long)idValue, Name = name, Type = type };

        // Tag doesn't exist, create a new one
        var tag = new Tag { Name = name, Type = type };
        var added = await _redis.HashSetAsync(typeKey, name, tag.Id, When.NotExists);

        if (!added)
            tag.Id = (long)await _redis.HashGetAsync(typeKey, name);

        return tag;
    }

    public async Task<List<Tag>> GetTagsByIdsAsync(IEnumerable<long> ids)
    {
        var result = new List<Tag>();

        foreach (var type in Enum.GetValues<TagType>())
        {
            var typeKey = $"tags:name:{type}";
            var allEntries = await _redis.HashGetAllAsync(typeKey);
            foreach (var id in ids)
            {
                var entry = allEntries.FirstOrDefault(e => (long)e.Value == id);
                if (!entry.Equals(new HashEntry()))
                {
                    result.Add(new Tag
                    {
                        Id = id,
                        Name = entry.Name,
                        Type = type
                    });
                }
            }
        }

        return result;
    }
}
