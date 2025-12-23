namespace Server.Contents.Api.Application.Commands;

public record CategoryDeleteCommand(long CategoryId) : IRequest<bool>;
public class CategoryDeleteCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryDeleteCommand, bool>
{
    public async Task<bool> Handle(CategoryDeleteCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories.FindAsync(command.CategoryId, cancellationToken);
        if (category is null)
            return false;
        category.IsDeleted = true;
        category.IsHidden = true;
        category.UpdatedAt = DateTime.UtcNow;
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}