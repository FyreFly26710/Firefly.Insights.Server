using System;
using MassTransit;
using Server.Messages.Contents;

namespace Server.Contents.Api.Infrastructure.Messaging;

public class CreateArticleRequestConsumer
    (ContentsContext _contentsContext, ILogger<CreateArticleRequestConsumer> _logger)
    : IConsumer<CreateArticleRequestMessage>
{
    public async Task Consume(ConsumeContext<CreateArticleRequestMessage> context)
    {
        var message = context.Message;
        try
        {
            var article = new Article()
            {
                Title = message.Title,
                Content = message.Content,
                Description = message.Description,
                ArticleMeta = new ArticleMeta()
                {
                    TopicId = message.TopicId,
                    IsTopicSummary = message.IsTopicSummary,
                    SortNumber = message.SortNumber,
                    UserId = message.UserId,
                },
            };
            // add article tags
            var skillLevel = await _contentsContext.UpsertTagAsync(message.SkillLevelTag, TagType.SkillLevel);
            var focusArea = await _contentsContext.UpsertTagAsync(message.FocusAreaTag, TagType.FocusArea);
            var articleStyle = await _contentsContext.UpsertTagAsync(message.ArticleStyleTag, TagType.ArticleStyle);
            var techStack = await _contentsContext.UpsertTagAsync(message.TechStackTag, TagType.TechStack);
            var tone = await _contentsContext.UpsertTagAsync(message.ToneTag, TagType.Tone);

            if (skillLevel is not null) article.ArticleMeta.ArticleTags.Add(new ArticleTag() { ArticleMetaId = article.ArticleMeta.Id, TagId = skillLevel.Id });
            if (focusArea is not null) article.ArticleMeta.ArticleTags.Add(new ArticleTag() { ArticleMetaId = article.ArticleMeta.Id, TagId = focusArea.Id });
            if (articleStyle is not null) article.ArticleMeta.ArticleTags.Add(new ArticleTag() { ArticleMetaId = article.ArticleMeta.Id, TagId = articleStyle.Id });
            if (techStack is not null) article.ArticleMeta.ArticleTags.Add(new ArticleTag() { ArticleMetaId = article.ArticleMeta.Id, TagId = techStack.Id });
            if (tone is not null) article.ArticleMeta.ArticleTags.Add(new ArticleTag() { ArticleMetaId = article.ArticleMeta.Id, TagId = tone.Id });

            _contentsContext.Articles.Add(article);
            await _contentsContext.SaveChangesAsync(context.CancellationToken);
            await context.RespondAsync(new CreateArticleRequestMessageResponse(article.Id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating article {Title}", message.Title);
            await context.RespondAsync(new CreateArticleRequestMessageResponse(-1));
        }
    }

}
