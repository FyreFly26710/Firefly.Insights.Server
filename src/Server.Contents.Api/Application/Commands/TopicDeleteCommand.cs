namespace Server.Contents.Api.Application.Commands;

public record TopicDeleteCommand(long TopicId) : IRequest<bool>;
public class TopicDeleteCommandHandler(ContentsContext _contentsContext) : IRequestHandler<TopicDeleteCommand, bool>
{
    public async Task<bool> Handle(TopicDeleteCommand command, CancellationToken cancellationToken)
    {
        var topic = await _contentsContext.Topics.FindAsync(command.TopicId, cancellationToken);
        if (topic is null)
            return false;
        topic.IsDeleted = true;
        topic.IsHidden = true;
        topic.UpdatedAt = DateTime.UtcNow;
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}