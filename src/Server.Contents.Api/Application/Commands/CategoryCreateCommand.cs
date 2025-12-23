namespace Server.Contents.Api.Application.Commands;

record CategoryCreateCommand(CategoryCreateRequest Request) : IRequest<long?>;

class CategoryCreateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryCreateCommand, long?>
{
    public async Task<long?> Handle(CategoryCreateCommand command, CancellationToken cancellationToken)
    {
        var maxSortNumber = await _contentsContext.Categories.MaxAsync(c => c.SortNumber, cancellationToken);
        var category = new Category()
        {
            Name = command.Request.Name,
            Description = command.Request.Description,
            ImageUrl = command.Request.ImageUrl,
            IsHidden = command.Request.IsHidden,
            SortNumber = maxSortNumber + 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _contentsContext.Categories.AddAsync(category, cancellationToken);
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}
