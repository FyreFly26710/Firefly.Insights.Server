
namespace Server.Contents.Api.Application.Commands;

public record CategoryCreateCommand(CategoryCreateRequest Request) : IRequest<long?>;
public class CategoryCreateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryCreateCommand, long?>
{
    public async Task<long?> Handle(CategoryCreateCommand command, CancellationToken cancellationToken)
    {
        var maxSortNumber = await _contentsContext.Categories.MaxAsync(c => (int?)c.SortNumber ?? 0, cancellationToken);
        var category = new Category()
        {
            Name = command.Request.Name,
            Description = command.Request.Description,
            ImageUrl = command.Request.ImageUrl,
            IsHidden = command.Request.IsHidden,
            SortNumber = maxSortNumber + 1,
        };
        await _contentsContext.Categories.AddAsync(category, cancellationToken);
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}

public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
{
    public CategoryCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(128).WithMessage("Category name cannot exceed 128 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(256).WithMessage("Description cannot exceed 256 characters.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(256).WithMessage("Image URL cannot exceed 256 characters.")
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Image URL must be a valid URL.");
    }
}