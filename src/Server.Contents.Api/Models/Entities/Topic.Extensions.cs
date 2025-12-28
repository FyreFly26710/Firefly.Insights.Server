using System;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Models.Entities;

public partial class Topic
{
    public TopicDto ToTopicDto() => new TopicDto()
    {
        TopicId = Id,
        Name = Name,
        Description = Description,
        CategoryId = CategoryId,
        CategoryName = Category.Name,
        ImageUrl = ImageUrl,
        SortNumber = SortNumber,
        IsHidden = IsHidden,
        TopicArticles = ArticleMetas.Select(
            a => new TopicArticleDto()
            {
                ArticleId = a.ArticleId,
                Title = a.Article.Title,
                Description = a.Article.Description,
                ImageUrl = a.ImageUrl,
                SortNumber = a.SortNumber,
                IsHidden = a.IsHidden,
                Tags = a.ArticleTags.Select(t => t.Tag.Name).ToList()
            }
        ).ToList()
    };
}
