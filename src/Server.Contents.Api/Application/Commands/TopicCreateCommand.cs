namespace Server.Contents.Api.Application.Commands;

record TopicCreateCommand(TopicCreateRequest Request) : IRequest<long?>;

class TopicCreateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<TopicCreateCommand, long?>
{
    public async Task<long?> Handle(TopicCreateCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
        if (category is null)
            throw new ExceptionNotFound($"Category of id {command.Request.CategoryId} not found");

        var maxSortNumber = await _contentsContext.Topics.MaxAsync(t => t.SortNumber, cancellationToken);
        var topic = new Topic()
        {
            Name = command.Request.Name,
            Description = command.Request.Description,
            CategoryId = command.Request.CategoryId,
            ImageUrl = command.Request.ImageUrl,
            SortNumber = maxSortNumber + 1,
        };
        await _contentsContext.Topics.AddAsync(topic, cancellationToken);
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return topic.Id;
    }
}
