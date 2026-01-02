using Server.Messages.Identities;

namespace Server.Contents.Api.Models.Entities;

public static class ArticleExtensions
{
    public static ArticleDto ToArticleDto(this Article article, UserTo userTo) => new ArticleDto()
    {
        ArticleId = article.Id,
        Title = article.Title,
        Content = article.Content,
        Description = article.Description,
        ImageUrl = article.ArticleMeta.ImageUrl,
        TopicId = article.ArticleMeta.TopicId,
        TopicName = article.ArticleMeta.Topic.Name,
        IsTopicSummary = article.ArticleMeta.IsTopicSummary,
        UserId = article.ArticleMeta.UserId,
        UserName = userTo.UserName,
        UserAvatar = userTo.UserAvatar,
        SortNumber = article.ArticleMeta.SortNumber,
        IsHidden = article.ArticleMeta.IsHidden,
        CreatedAt = article.ArticleMeta.CreatedAt,
        UpdatedAt = article.ArticleMeta.UpdatedAt,
        Tags = article.ArticleMeta.ArticleTags.Select(t => t.Tag.ToTagDto()).ToList()
    };
}
