using Microsoft.Extensions.Logging;
using NSubstitute;
using Server.Contents.Api.Application.Queries;
using Server.Contents.Api.Infrastructure.RedisRepositories;
using StackExchange.Redis;

namespace Server.Contents.Tests.Application.Queries;

public class ArticleQueriesTests : IDisposable
{
    private readonly ContentsContext _context;
    private readonly RedisTagRepository _tagRepository;
    private readonly IConnectionMultiplexer _redis;
    private readonly ArticleQueries _queries;

    public ArticleQueriesTests()
    {
        _context = TestUtils.CreateInMemoryDbContext();
        _redis = TestUtils.CreateTestRedisDb();
        _tagRepository = new RedisTagRepository(_redis);
        _queries = new ArticleQueries(_context, _tagRepository, Substitute.For<ILogger<ArticleQueries>>());

        ClearRedis();
    }

    [Fact]
    public async Task GetArticleById_ValidId_ReturnsDtoWithRedisTags()
    {
        // Arrange
        var topic = new Topic { Id = 1, Name = "Architecture" };
        var article = new Article
        {
            Id = 100,
            Title = "Testing Redis",
            Content = "Content",
            ArticleMeta = new ArticleMeta
            {
                Topic = topic,
                ArticleTags = new List<ArticleTag> { new() { TagId = 1 } }
            }
        };
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        _tagRepository.AddRange(new List<Tag>
        {
            new() { Id = 1, Name = "Redis", Type = TagType.SkillLevel }
        });

        // Act
        var result = await _queries.GetArticleById(100);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Testing Redis", result.Title);
        Assert.Single(result.Tags);
        Assert.Equal("Redis", result.Tags[0].Name);
    }

    [Fact]
    public async Task GetArticleById_NonExistent_ThrowsExceptionNotFound()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ExceptionNotFound>(() => _queries.GetArticleById(999));
    }

    [Fact]
    public async Task GetArticleList_WithTitleFilter_ReturnsFilteredResults()
    {
        // Arrange
        var topic = new Topic { Id = 2, Name = "Dev" };
        _context.Articles.AddRange(
            new Article { Id = 200, Title = "Match", ArticleMeta = new ArticleMeta { Topic = topic } },
            new Article { Id = 201, Title = "Ignore", ArticleMeta = new ArticleMeta { Topic = topic } }
        );
        await _context.SaveChangesAsync();

        var request = new ArticleListRequest { ArticleTitle = "Match", PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _queries.GetArticleList(request);

        // Assert
        Assert.Single(result.Data);
        Assert.Equal("Match", result.Data[0].Title);
    }

    [Fact]
    public async Task GetArticleList_AggregatesTagsFromRedis()
    {
        // Arrange
        var topic = new Topic { Id = 3, Name = "General" };
        var article = new Article
        {
            Id = 301,
            Title = "Tagged Article",
            ArticleMeta = new ArticleMeta
            {
                Topic = topic,
                ArticleTags = new List<ArticleTag> { new() { TagId = 55 } }
            }
        };

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        _tagRepository.AddRange(new List<Tag>
        {
            new() { Id = 55, Name = "SharedTag", Type = TagType.SkillLevel }
        });

        // Act
        var result = await _queries.GetArticleList(new ArticleListRequest { PageNumber = 1, PageSize = 10 });

        // Assert
        var dto = result.Data.First();
        Assert.Equal("SharedTag", dto.Tags.First().Name);
    }

    private void ClearRedis()
    {
        _redis.ClearTestRedisDb();
    }

    public void Dispose()
    {
        ClearRedis();
        _redis.Dispose();
        _context.Dispose();
    }
}