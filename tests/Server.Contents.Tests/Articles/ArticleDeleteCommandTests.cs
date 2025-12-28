namespace Server.Contents.Tests.Articles;

public class ArticleDeleteCommandTests
{
    private readonly ContentsContext _contentsContext;
    private readonly ArticleDeleteCommandHandler _handler;
    public ArticleDeleteCommandTests()
    {
        _contentsContext = TestUtils.CreateInMemoryDbContext();
        _handler = new ArticleDeleteCommandHandler(_contentsContext);
    }
    [Fact]
    public async Task Handle_WithValidArticle_SetsIsDeletedFlags()
    {
        // Arrange

        var article = new Article
        {
            Id = 1,
            Title = "To be deleted",
            ArticleMeta = new ArticleMeta
            {
                Id = 1,
                ArticleTags = new List<ArticleTag>
                {
                    new() { Id = 1 },
                    new() { Id = 2 }
                }
            }
        };
        _contentsContext.Articles.Add(article);
        await _contentsContext.SaveChangesAsync();

        var command = new ArticleDeleteCommand(article.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);

        // Use IgnoreQueryFilters to bypass soft delete filter and check flags
        var deletedArticle = await _contentsContext.Articles
            .IgnoreQueryFilters()
            .Include(a => a.ArticleMeta)
            .ThenInclude(am => am.ArticleTags)
            .FirstAsync(a => a.Id == article.Id);

        Assert.True(deletedArticle.IsDeleted);
        Assert.True(deletedArticle.ArticleMeta.IsDeleted);

        foreach (var tag in deletedArticle.ArticleMeta.ArticleTags)
        {
            Assert.True(tag.IsDeleted);
        }
    }

    [Fact]
    public async Task Handle_NonExistentArticle_ThrowsExceptionNotFound()
    {
        // Arrange
        var command = new ArticleDeleteCommand(999);

        // Act & Assert
        await Assert.ThrowsAsync<ExceptionNotFound>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
