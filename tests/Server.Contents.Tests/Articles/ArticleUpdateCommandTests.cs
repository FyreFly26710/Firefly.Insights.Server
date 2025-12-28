namespace Server.Contents.Tests.Articles;

public class ArticleUpdateCommandTests
{
    private readonly ArticleUpdateRequestValidator _validator;
    private readonly ContentsContext _contentsContext;
    private readonly ArticleUpdateCommandHandler _handler;
    public ArticleUpdateCommandTests()
    {
        _validator = new ArticleUpdateRequestValidator();
        _contentsContext = TestUtils.CreateInMemoryDbContext();
        _handler = new ArticleUpdateCommandHandler(_contentsContext);
    }

    private readonly ArticleUpdateRequest _fullValidRequest = new ArticleUpdateRequest(
        ArticleId: 1,
        Title: "Valid Title",
        Content: "Valid content",
        TopicId: 1,
        Description: "Valid description",
        ImageUrl: "https://example.com/image.png",
        IsTopicSummary: true,
        SortNumber: 1,
        IsHidden: false,
        TagIds: [1, 2]
    );

    #region Handler Tests
    [Fact]
    public async Task Handle_WithValidRequest_SkipNullProperties()
    {
        // Arrange
        _contentsContext.Topics.Add(new Topic { Id = 1, Name = "Test Topic" });
        await _contentsContext.SaveChangesAsync();

        var article = new Article
        {
            Id = 1,
            Title = "Old Title",
            Content = "Old Content",
            Description = "Old Description",
            ArticleMeta = new ArticleMeta
            {
                TopicId = 1,
                UserId = 1,
                SortNumber = 5,
                IsHidden = false,
                IsTopicSummary = false
            }
        };
        _contentsContext.Articles.Add(article);
        await _contentsContext.SaveChangesAsync();

        var request = new ArticleUpdateRequest(ArticleId: article.Id, Title: "New Title", Content: "New Content");
        var command = new ArticleUpdateCommand(request);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);

        var updated = await _contentsContext.Articles
            .Include(a => a.ArticleMeta)
            .ThenInclude(am => am.ArticleTags)
            .ThenInclude(at => at.Tag)
            .FirstAsync(a => a.Id == article.Id);

        Assert.Equal("New Title", updated.Title);
        Assert.Equal("New Content", updated.Content);
        Assert.Equal(article.Description, updated.Description);
        Assert.Equal(article.ArticleMeta.ImageUrl, updated.ArticleMeta.ImageUrl);
        Assert.Equal(article.ArticleMeta.IsHidden, updated.ArticleMeta.IsHidden);
        Assert.Equal(article.ArticleMeta.IsTopicSummary, updated.ArticleMeta.IsTopicSummary);
        Assert.Equal(article.ArticleMeta.SortNumber, updated.ArticleMeta.SortNumber);
        Assert.Equal(article.ArticleMeta.TopicId, updated.ArticleMeta.TopicId);
    }
    [Fact]
    public async Task Handle_WithValidRequest_UpdatesArticle()
    {
        // Arrange
        _contentsContext.Tags.Add(new Tag { Id = 1, Name = "Tag1", Type = TagType.Default });
        _contentsContext.Tags.Add(new Tag { Id = 2, Name = "Tag2", Type = TagType.Default });
        _contentsContext.Topics.Add(new Topic { Id = 1, Name = "Test Topic" }); 
        await _contentsContext.SaveChangesAsync();

        var article = new Article
        {
            Id = 1,
            Title = "Old Title",
            Content = "Old Content",
            Description = "Old Description",
            ArticleMeta = new ArticleMeta
            {
                TopicId = 1,
                UserId = 1,
                SortNumber = 5,
                IsHidden = false,
                IsTopicSummary = false
            }
        };
        _contentsContext.Articles.Add(article);
        await _contentsContext.SaveChangesAsync();

        var request = _fullValidRequest with { Title = "New Title", Content = "New Content" };
        var command = new ArticleUpdateCommand(request);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);

        var updated = await _contentsContext.Articles
            .Include(a => a.ArticleMeta)
            .FirstAsync(a => a.Id == article.Id);

        Assert.Equal("New Title", updated.Title);
        Assert.Equal("New Content", updated.Content);
        Assert.Equal(request.Description, updated.Description);
        Assert.Equal(request.ImageUrl, updated.ArticleMeta.ImageUrl);
        Assert.Equal(request.IsHidden, updated.ArticleMeta.IsHidden);
        Assert.Equal(request.IsTopicSummary, updated.ArticleMeta.IsTopicSummary);
        Assert.Equal(request.SortNumber, updated.ArticleMeta.SortNumber);
        Assert.Equal(request.TopicId, updated.ArticleMeta.TopicId);
        Assert.Equal(request.TagIds.Count, updated.ArticleMeta.ArticleTags.Count);

    }

    [Fact]
    public async Task Handle_WithNonExistentArticle_ThrowsExceptionNotFound()
    {
        // Arrange

        var request = _fullValidRequest with { ArticleId = 999 };
        var command = new ArticleUpdateCommand(request);

        // Act & Assert
        await Assert.ThrowsAsync<ExceptionNotFound>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistentTopic_ThrowsExceptionNotFound()
    {
        // Arrange

        var article = new Article
        {
            Id = 1,
            Title = "Old Title",
            Content = "Old Content",
            ArticleMeta = new ArticleMeta
            {
                TopicId = 1,
                UserId = 1
            }
        };
        _contentsContext.Articles.Add(article);
        await _contentsContext.SaveChangesAsync();

        var request = _fullValidRequest with { TopicId = 999 };
        var command = new ArticleUpdateCommand(request);

        // Act & Assert
        await Assert.ThrowsAsync<ExceptionNotFound>(() =>
            _handler.Handle(command, CancellationToken.None));
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
        Assert.Contains(result1.Errors, e => e.PropertyName == nameof(ArticleUpdateRequest.Title));

        // Too long title
        var request2 = _fullValidRequest with { Title = new string('a', 129) };
        var result2 = _validator.Validate(request2);
        Assert.False(result2.IsValid);
        Assert.Contains(result2.Errors, e => e.PropertyName == nameof(ArticleUpdateRequest.Title));
    }

    [Fact]
    public void Validator_Description_FailsValidation()
    {
        var request = _fullValidRequest with { Description = new string('a', 257) };
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(ArticleUpdateRequest.Description));
    }

    [Fact]
    public void Validator_ImageUrl_FailsValidation()
    {
        var request1 = _fullValidRequest with { ImageUrl = "not-a-valid-url" };
        var result1 = _validator.Validate(request1);
        Assert.False(result1.IsValid);
        Assert.Contains(result1.Errors, e => e.PropertyName == nameof(ArticleUpdateRequest.ImageUrl));

        var request2 = _fullValidRequest with { ImageUrl = new string('a', 257) };
        var result2 = _validator.Validate(request2);
        Assert.False(result2.IsValid);
        Assert.Contains(result2.Errors, e => e.PropertyName == nameof(ArticleUpdateRequest.ImageUrl));
    }

    #endregion
}
