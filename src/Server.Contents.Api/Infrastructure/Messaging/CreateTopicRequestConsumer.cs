using System;
using MassTransit;
using Server.Messages.Contents;

namespace Server.Contents.Api.Infrastructure.Messaging;

public class CreateTopicRequestConsumer(ContentsContext _contentsContext, ILogger<CreateTopicRequestConsumer> _logger) : IConsumer<CreateTopicRequestMessage>
{
    public async Task Consume(ConsumeContext<CreateTopicRequestMessage> context)
    {
        var message = context.Message;
        try
        {
            var topics = await _contentsContext.Topics.Where(t => t.CategoryId == message.CategoryId).ToListAsync(context.CancellationToken);
            var sortNumber = topics.Count > 0 ? topics.Max(t => t.SortNumber) + 1 : 0;
            var topic = new Topic()
            {
                Name = message.Topic,
                Description = message.TopicDescription,
                CategoryId = message.CategoryId,
                SortNumber = sortNumber,
                IsHidden = false,
                ImageUrl = message.TopicUrl,
            };
            _contentsContext.Topics.Add(topic);
            await _contentsContext.SaveChangesAsync(context.CancellationToken);

            var response = new CreateTopicRequestMessageResponse(topic.Id);
            await context.RespondAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating topic {TopicName}", message.Topic);
            await context.RespondAsync(new CreateTopicRequestMessageResponse(-1));
        }
    }
}
