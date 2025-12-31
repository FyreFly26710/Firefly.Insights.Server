namespace Server.Contents.Api.Models.Entities;

public static class TopicExtensions
{
    public static TopicDto ToTopicDto(this Topic topic) => new TopicDto()
    {
        TopicId = topic.Id,
        Name = topic.Name,
        Description = topic.Description,
        CategoryId = topic.CategoryId,
        CategoryName = topic.Category.Name,
        ImageUrl = topic.ImageUrl,
        SortNumber = topic.SortNumber,
        IsHidden = topic.IsHidden,
        CreatedAt = topic.CreatedAt,
        UpdatedAt = topic.UpdatedAt,
        TopicArticles = topic.ArticleMetas.Select(a => new TopicArticleDto()
        {
            ArticleId = a.ArticleId,
            Title = a.Article.Title,
            Description = a.Article.Description,
            ImageUrl = a.ImageUrl,
            SortNumber = a.SortNumber,
            IsHidden = a.IsHidden,
            IsTopicSummary = a.IsTopicSummary,
        }).ToList()
    };
}
