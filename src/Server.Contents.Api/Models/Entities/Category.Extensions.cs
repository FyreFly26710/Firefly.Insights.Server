using System;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Models.Entities;

public partial class Category
{
    public CategoryDto ToCategoryDto() => new CategoryDto()
    {
        CategoryId = Id,
        Name = Name,
        Description = Description,
        ImageUrl = ImageUrl,
        SortNumber = SortNumber,
        IsHidden = IsHidden,
        CategoryTopics = Topics.Select(t => new CategoryTopicDto()
        {
            TopicId = t.Id,
            Name = t.Name,
            Description = t.Description,
            ImageUrl = t.ImageUrl,
            SortNumber = t.SortNumber,
            IsHidden = t.IsHidden
        }).ToList()
    };
}
