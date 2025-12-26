namespace Server.Contents.FunctionalTests.Articles;

public class ArticleCreateCommandTests
{
    private readonly ArticleCreateRequestValidator _validator;
    public ArticleCreateCommandTests() { _validator = new ArticleCreateRequestValidator(); }
    private readonly ArticleCreateRequest _fullValidRequest = new ArticleCreateRequest(
        Title: "Valid Title",
        TopicId: 1,
        Description: "Valid description",
        Content: "Valid content",
        ImageUrl: "https://example.com/image.png",
        IsTopicSummary: true,
        SortNumber: 1,
        IsHidden: false
        );


    #region Handler Tests

    private ContentsContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ContentsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ContentsContext(options);
    }

    [Fact]
    public async Task Handle_WithValidRequest_CreatesArticle()
    {
        // Arrange
        await using var context = CreateInMemoryDbContext();
        var topic = new Topic { Id = 1, Name = "Test Topic" };
        context.Topics.Add(topic);
        await context.SaveChangesAsync();

        var handler = new ArticleCreateCommandHandler(context);
        var request = _fullValidRequest with { Title = "My Article", Content = "Hello World", TopicId = topic.Id };
        var command = new ArticleCreateCommand(request);

        // Act
        var articleId = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(articleId);

        var article = await context.Articles
            .Include(a => a.ArticleMeta)
            .FirstOrDefaultAsync(a => a.Id == articleId);

        Assert.NotNull(article);
        Assert.Equal("My Article", article!.Title);
        Assert.Equal("Hello World", article.Content);
        Assert.Equal(topic.Id, article.ArticleMeta.TopicId);
        Assert.Equal(request.SortNumber, article.ArticleMeta.SortNumber);
        Assert.Equal(request.IsHidden, article.ArticleMeta.IsHidden);
        Assert.Equal(request.IsTopicSummary, article.ArticleMeta.IsTopicSummary);
        Assert.Equal(request.ImageUrl, article.ArticleMeta.ImageUrl);
        Assert.Equal(request.Description, article.Description);
    }

    [Fact]
    public async Task Handle_WithNonExistentTopic_ThrowsExceptionNotFound()
    {
        // Arrange
        await using var context = CreateInMemoryDbContext();
        var handler = new ArticleCreateCommandHandler(context);

        var request = new ArticleCreateRequest
        (
            Title: "Invalid",
            TopicId: 999 // topic does not exist
        );

        var command = new ArticleCreateCommand(request);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ExceptionNotFound>(async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        });

        Assert.Equal("Topic of id 999 not found", ex.Message);
    }

    #endregion


    #region Validator Tests

    [Fact]
    public void Validator_AllValidProperties_PassesValidation()
    {
        var result = _validator.Validate(_fullValidRequest);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_Title_FailsValidation()
    {
        // Empty title
        var request1 = _fullValidRequest with { Title = "" };
        var result1 = _validator.Validate(request1);
        Assert.False(result1.IsValid);
        Assert.Contains(result1.Errors, e => e.PropertyName == nameof(ArticleCreateRequest.Title));

        // Too long title
        var request2 = _fullValidRequest with { Title = new string('a', 129) };
        var result2 = _validator.Validate(request2);
        Assert.False(result2.IsValid);
        Assert.Contains(result2.Errors, e => e.PropertyName == nameof(ArticleCreateRequest.Title));
    }

    [Fact]
    public void Validator_Description_FailsValidation()
    {
        var request = _fullValidRequest with { Description = new string('a', 257) };
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ArticleCreateRequest.Description));
    }

    [Fact]
    public void Validator_ImageUrl_FailsValidation()
    {
        // Invalid URL
        var request1 = _fullValidRequest with { ImageUrl = "not-a-valid-url" };
        var result1 = _validator.Validate(request1);
        Assert.False(result1.IsValid);
        Assert.Contains(result1.Errors, e => e.PropertyName == nameof(ArticleCreateRequest.ImageUrl));

        // Too long URL
        var request2 = _fullValidRequest with { ImageUrl = new string('a', 257) };
        var result2 = _validator.Validate(request2);
        Assert.False(result2.IsValid);
        Assert.Contains(result2.Errors, e => e.PropertyName == nameof(ArticleCreateRequest.ImageUrl));
    }

    [Fact]
    public void Validator_TopicId_FailsValidation()
    {
        var request = _fullValidRequest with { TopicId = 0 };
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ArticleCreateRequest.TopicId));
    }

    #endregion
}
