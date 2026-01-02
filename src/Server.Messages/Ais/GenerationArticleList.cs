using System;

namespace Server.Messages.Ais;

public record GenerationArticleList
(
    List<GenerationArticleSummary> Articles,
    string AiMessage
);
