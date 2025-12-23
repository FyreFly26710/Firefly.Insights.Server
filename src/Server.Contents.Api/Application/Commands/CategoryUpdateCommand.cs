namespace Server.Contents.Api.Application.Commands;
record CategoryUpdateCommand(CategoryUpdateRequest Request) : IRequest<bool>;

class CategoryUpdateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryUpdateCommand, bool>
{
    public async Task<bool> Handle(CategoryUpdateCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
        if (category is null)
            return false;

        category.Name = command.Request.Name;
        category.Description = command.Request.Description;
        category.ImageUrl = command.Request.ImageUrl;
        category.IsHidden = command.Request.IsHidden;
        category.SortNumber = command.Request.SortNumber;
        category.UpdatedAt = DateTime.UtcNow;

        await _contentsContext.SaveChangesAsync(cancellationToken);

        return true;
    }

}
