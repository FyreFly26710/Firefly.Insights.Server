namespace Server.Contents.Api.Application.Commands;

public record CategoryDeleteCommand(long CategoryId) : IRequest<bool>;
public class CategoryDeleteCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryDeleteCommand, bool>
{
    public async Task<bool> Handle(CategoryDeleteCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories
            .Include(c => c.Topics)
            .FirstOrDefaultAsync(c => c.Id == command.CategoryId, cancellationToken);
        if (category is null)
            throw new ExceptionNotFound($"Category of id {command.CategoryId} not found");
        category.IsDeleted = true;
        category.UpdatedAt = DateTime.UtcNow;
        foreach (var topic in category.Topics)
        {
            topic.CategoryId = null;
            topic.UpdatedAt = DateTime.UtcNow;
        }
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}