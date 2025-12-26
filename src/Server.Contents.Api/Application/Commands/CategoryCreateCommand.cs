
using Server.Contents.Api.Infrastructure.EfContexts;

namespace Server.Contents.Api.Application.Commands;

public record CategoryCreateCommand(CategoryCreateRequest Request) : IRequest<long?>;
public class CategoryCreateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryCreateCommand, long?>
{
    public async Task<long?> Handle(CategoryCreateCommand command, CancellationToken cancellationToken)
    {
        var category = new Category()
        {
            Name = command.Request.Name,
            Description = command.Request.Description,
            ImageUrl = command.Request.ImageUrl,
            SortNumber = command.Request.SortNumber,
            IsHidden = command.Request.IsHidden,
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