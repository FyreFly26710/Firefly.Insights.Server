namespace Server.Messages.Ais;

public record GenerationArticleSummary
(
    int SortNumber,
    string Title,
    string Description,
    // List<string> Tags
    string SkillLevelTag,
    string FocusAreaTag,
    string ArticleStyleTag,
    string TechStackTag,
    string ToneTag
);