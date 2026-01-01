using System;

namespace Server.Messages.Ais;

public record GenerationArticleList
(
    List<GenerationArticleSummary> Articles,
    string AiMessage,
    long TopicId = 0
);

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