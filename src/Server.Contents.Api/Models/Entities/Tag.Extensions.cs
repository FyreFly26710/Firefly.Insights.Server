namespace Server.Contents.Api.Models.Entities;

public static class TagExtensions
{
    public static TagDto ToTagDto(this Tag tag) => new TagDto()
    {
        TagId = tag.Id,
        Name = tag.Name,
        Type = tag.Type.ToString()
    };
}
