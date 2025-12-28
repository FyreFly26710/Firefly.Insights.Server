namespace Server.Contents.Api.Models.Entities;

public class Tag : Entity
{
    public string Name { get; set; } = "";
    public TagType Type { get; set; }
    // public ICollection<ArticleTag> ArticleTags { get; set; } = [];

}

public enum TagType
{
    Default = 0,
    SkillLevel = 1,
    ArticleStyle = 2,
    FocusArea = 3,
    TechStack = 4,
    Tone = 5,
}
public static class TagTypeExtensions
{
    public static string ToText(this TagType type)
    {
        return type switch
        {
            TagType.Tone => "Tone",
            TagType.SkillLevel => "Skill Level",
            TagType.ArticleStyle => "Article Style",
            TagType.FocusArea => "Focus Area",
            TagType.TechStack => "Tech Stack",
            _ => type.ToString()
        };
    }
}