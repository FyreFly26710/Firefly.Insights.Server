using Server.Contents.Api.Infrastructure.EfContexts;

namespace Server.Contents.Api.Application.Commands;

public record TopicDeleteCommand(long TopicId) : IRequest<bool>;
public class TopicDeleteCommandHandler(ContentsContext _contentsContext) : IRequestHandler<TopicDeleteCommand, bool>
{
    public async Task<bool> Handle(TopicDeleteCommand command, CancellationToken cancellationToken)
    {
        var topic = await _contentsContext.Topics
            .Include(t => t.ArticleMetas)
            .FirstOrDefaultAsync(t => t.Id == command.TopicId, cancellationToken);
        if (topic is null)
            throw new ExceptionNotFound($"Topic of id {command.TopicId} not found");
        topic.IsDeleted = true;
        topic.UpdatedAt = DateTime.UtcNow;
        foreach (var articleMeta in topic.ArticleMetas)
        {
            articleMeta.TopicId = 0;
            articleMeta.UpdatedAt = DateTime.UtcNow;
        }
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}