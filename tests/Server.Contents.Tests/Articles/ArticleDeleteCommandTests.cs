namespace Server.Contents.Tests.Articles;

public class ArticleDeleteCommandTests
{

    [Fact]
    public async Task Handle_WithValidArticle_SetsIsDeletedFlags()
    {
        // Arrange
        await using var context = TestUtils.CreateInMemoryDbContext();

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
        context.Articles.Add(article);
        await context.SaveChangesAsync();

        var handler = new ArticleDeleteCommandHandler(context);
        var command = new ArticleDeleteCommand(article.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);

        // Use IgnoreQueryFilters to bypass soft delete filter and check flags
        var deletedArticle = await context.Articles
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
        await using var context = TestUtils.CreateInMemoryDbContext();
        var handler = new ArticleDeleteCommandHandler(context);
        var command = new ArticleDeleteCommand(999);

        // Act & Assert
        await Assert.ThrowsAsync<ExceptionNotFound>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}
