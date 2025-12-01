using System;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Models.Entities;

public partial class Article
{
    public ArticleDto ToArticleDto() => new ArticleDto()
    {
        ArticleId = Id,
        Title = Title,
        Content = Content,
        Description = Description,

        ImageUrl = ArticleMeta.ImageUrl ?? "",
        TopicId = ArticleMeta.TopicId,
        TopicName = ArticleMeta.Topic.Name,
        IsTopicSummary = ArticleMeta.IsTopicSummary,
        UserId = ArticleMeta.UserId,
        UserName = "",
        SortNumber = ArticleMeta.SortNumber,
        IsHidden = ArticleMeta.IsHidden,
        CreatedAt = ArticleMeta.CreatedAt,
        UpdatedAt = ArticleMeta.UpdatedAt,
        Tags = ArticleMeta.ArticleTags.Select(t => t.Tag.Name).ToList()
    };
}
