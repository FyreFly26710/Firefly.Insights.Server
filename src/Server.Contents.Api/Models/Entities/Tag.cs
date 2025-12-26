using Server.Common.Types;

namespace Server.Contents.Api.Models.Entities;

public class Tag : Entity
{
    public string Name { get; set; } = "";
    public TagType Type { get; set; }
    public ICollection<ArticleTag> ArticleTags { get; set; } = [];

}

public enum TagType
{
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

    // These tags are static and should not be deleted
    public static List<Tag> GetStaticTags() => [
        new Tag() { Id = 1, Name = "Beginner", Type = TagType.SkillLevel},
        new Tag() { Id = 2, Name = "Advanced", Type = TagType.SkillLevel},
        new Tag() { Id = 3, Name = "Expert", Type = TagType.SkillLevel},
        new Tag() { Id = 4, Name = "General", Type = TagType.SkillLevel},

        new Tag() { Id = 5, Name = "Overview", Type = TagType.ArticleStyle},
        new Tag() { Id = 6, Name = "Deep-dive", Type = TagType.ArticleStyle},
        new Tag() { Id = 7, Name = "Best-practices", Type = TagType.ArticleStyle},
        new Tag() { Id = 8, Name = "Listicle", Type = TagType.ArticleStyle},
        new Tag() { Id = 9, Name = "Q&A", Type = TagType.ArticleStyle},
        new Tag() { Id = 10, Name = "Comparison", Type = TagType.ArticleStyle},

        new Tag() { Id = 11, Name = "Conversational", Type = TagType.Tone},
        new Tag() { Id = 12, Name = "Academic", Type = TagType.Tone},
        new Tag() { Id = 13, Name = "Technical", Type = TagType.Tone},
        new Tag() { Id = 14, Name = "Code-heavy", Type = TagType.Tone},

    ];
}