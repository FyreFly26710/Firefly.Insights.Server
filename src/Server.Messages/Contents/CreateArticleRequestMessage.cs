using System;
using MassTransit.Mediator;

namespace Server.Messages.Contents;
public record CreateArticleRequestMessage
(
    string Title,
    long TopicId,
    long UserId,
    string Content,

    string Description = "",
    int SortNumber = 0,
    string SkillLevelTag = "",
    string FocusAreaTag = "",
    string ArticleStyleTag = "",
    string TechStackTag = "",
    string ToneTag = "",
    bool IsTopicSummary = true
    ) : Request<CreateArticleRequestMessageResponse>;

public record CreateArticleRequestMessageResponse(long ArticleId);