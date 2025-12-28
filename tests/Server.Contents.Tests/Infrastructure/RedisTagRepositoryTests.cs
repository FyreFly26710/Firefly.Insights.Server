using Server.Contents.Api.Infrastructure.RedisRepositories;
using StackExchange.Redis;

namespace Server.Contents.Tests.Infrastructure;

public class RedisTagRepositoryTests : IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly RedisTagRepository _repository;

    public RedisTagRepositoryTests()
    {
        _redis = TestUtils.CreateTestRedisDb();
        _repository = new RedisTagRepository(_redis);
        ClearDatabase();
    }

    [Fact]
    public async Task GetOrAddTagAsync_WhenNew_CreatesAndReturnsTag()
    {
        // Arrange
        string tagName = "DotNet";
        TagType type = TagType.SkillLevel;

        // Act
        var result = await _repository.GetOrAddTagAsync(tagName, type);

        // Assert
        Assert.Equal(tagName, result.Name);
        Assert.Equal(type, result.Type);
        Assert.True(result.Id > 0);

        // Verify it actually exists in Redis
        var db = _redis.GetDatabase();
        var exists = await db.HashExistsAsync($"tags:name:{type}", tagName);
        Assert.True(exists);
    }

    [Fact]
    public async Task GetOrAddTagAsync_WhenExists_ReturnsExistingTag()
    {
        // Arrange
        var initial = await _repository.GetOrAddTagAsync("React", TagType.SkillLevel);

        // Act
        var secondCall = await _repository.GetOrAddTagAsync("React", TagType.SkillLevel);

        // Assert
        Assert.Equal(initial.Id, secondCall.Id);
    }

    [Fact]
    public async Task AddRangeAsync_MultipleTypes_PersistsAll()
    {
        // Arrange
        var tags = new List<Tag>
        {
            new() { Id = 101, Name = "C#", Type = TagType.SkillLevel },
            new() { Id = 102, Name = "Java", Type = TagType.SkillLevel },
            new() { Id = 201, Name = "Tutorial", Type = TagType.ArticleStyle }
        };

        // Act
        _repository.AddRange(tags);

        // Assert
        var skillTags = await _repository.GetTagsByIdsAsync(new List<long> { 101, 102 });
        var categoryTags = await _repository.GetTagsByIdsAsync(new List<long> { 201 });

        Assert.Equal(2, skillTags.Count);
        Assert.Single(categoryTags);
        Assert.Contains(skillTags, t => t.Name == "C#");
    }

    [Fact]
    public async Task GetTagsByIdsAsync_FilterExisting_ReturnsOnlyMatches()
    {
        // Arrange
        var tags = new List<Tag>
        {
            new() { Id = 50, Name = "Redis", Type = TagType.SkillLevel },
            new() { Id = 60, Name = "Docker", Type = TagType.SkillLevel }
        };
        _repository.AddRange(tags);

        // Act
        var result = await _repository.GetTagsByIdsAsync(new List<long> { 50, 999 }); // 999 doesn't exist

        // Assert
        Assert.Single(result);
        Assert.Equal("Redis", result[0].Name);
    }

    private void ClearDatabase()
    {
        _redis.ClearTestRedisDb();
    }

    public void Dispose()
    {
        ClearDatabase();
        _redis.Dispose();
    }
}