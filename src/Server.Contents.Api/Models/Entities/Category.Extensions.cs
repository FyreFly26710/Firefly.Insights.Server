namespace Server.Contents.Api.Models.Entities;

public static class CategoryExtensions
{
    public static CategoryDto ToCategoryDto(this Category category) => new CategoryDto()
    {
        CategoryId = category.Id,
        Name = category.Name,
        Description = category.Description,
        ImageUrl = category.ImageUrl,
        SortNumber = category.SortNumber,
        IsHidden = category.IsHidden,
        CategoryTopics = category.Topics.Select(t => new CategoryTopicDto()
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
