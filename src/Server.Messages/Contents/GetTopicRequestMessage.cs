using System;
using MassTransit.Mediator;

namespace Server.Messages.Contents;

public record GetTopicRequestMessage(long TopicId, bool WithArticles = false) : Request<GetTopicRequestMessageResponse>;

public record GetTopicRequestMessageResponse(TopicTo Topic);