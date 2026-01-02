using System;
using MassTransit.Mediator;

namespace Server.Messages.Contents;

public record CreateTopicRequestMessage(long CategoryId, string Topic, string TopicDescription, string TopicUrl) : Request<CreateTopicRequestMessageResponse>;

public record CreateTopicRequestMessageResponse(long TopicId);