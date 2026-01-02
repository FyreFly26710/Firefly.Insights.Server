using System;
using MassTransit;
using Server.Messages.Contents;

namespace Server.Contents.Api.Infrastructure.Messaging;

public class GetTopicRequestConsumer(ContentsContext _contentsContext, ILogger<GetTopicRequestConsumer> _logger) : IConsumer<GetTopicRequestMessage>
{
    public async Task Consume(ConsumeContext<GetTopicRequestMessage> context)
    {
        var message = context.Message;
        var query = _contentsContext.Topics.AsQueryable().AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.ArticleMetas).ThenInclude(am => am.Article);
        var topic = await query.FirstOrDefaultAsync(t => t.Id == message.TopicId, context.CancellationToken);
        if (topic is null)
        {
            _logger.LogError("Topic not found: {TopicId}", message.TopicId);
            await context.RespondAsync(new GetTopicRequestMessageResponse(new TopicTo()));
            return;
        }
        var topicTo = new TopicTo()
        {
            TopicId = topic.Id,
            Name = topic.Name,
            Description = topic.Description,
            CategoryId = topic.CategoryId,
            CategoryName = topic.Category.Name,
            CategoryDescription = topic.Category.Description,
        };
        if (topic.ArticleMetas.Count > 0 && message.WithArticles)
        {
            topicTo.TopicArticles = topic.ArticleMetas.Select(am => new TopicArticleTo()
            {
                ArticleId = am.ArticleId,
                Title = am.Article.Title,
                Description = am.Article.Description,
                SortNumber = am.SortNumber,
                Tags = am.ArticleTags.Select(at => at.Tag.Name).ToList(),
            }).ToList();
            topicTo.TopicArticles = topicTo.TopicArticles.OrderBy(ta => ta.SortNumber).ToList();
        }
        await context.RespondAsync(new GetTopicRequestMessageResponse(topicTo));
    }

}
