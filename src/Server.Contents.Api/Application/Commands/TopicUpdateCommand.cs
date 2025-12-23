namespace Server.Contents.Api.Application.Commands;

record TopicUpdateCommand(TopicUpdateRequest Request) : IRequest<bool>;

class TopicUpdateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<TopicUpdateCommand, bool>
{
    public async Task<bool> Handle(TopicUpdateCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
        if (category is null)
            throw new ExceptionNotFound($"Category of id {command.Request.CategoryId} not found");

        var topic = await _contentsContext.Topics.FindAsync(command.Request.TopicId, cancellationToken);
        if (topic is null)
            throw new ExceptionNotFound($"Topic of id {command.Request.TopicId} not found");

        topic.Name = command.Request.Name;
        topic.Description = command.Request.Description;
        topic.CategoryId = command.Request.CategoryId;
        topic.ImageUrl = command.Request.ImageUrl;
        topic.SortNumber = command.Request.SortNumber;
        topic.IsHidden = command.Request.IsHidden;
        topic.UpdatedAt = DateTime.UtcNow;

        await _contentsContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
