namespace Server.Contents.Api.Application.Commands;
public record CategoryUpdateCommand(CategoryUpdateRequest Request) : IRequest<bool>;
public class CategoryUpdateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryUpdateCommand, bool>
{
    public async Task<bool> Handle(CategoryUpdateCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
        if (category is null)
            throw new ExceptionNotFound($"Category of id {command.Request.CategoryId} not found");

        category.Name = command.Request.Name ?? category.Name;
        category.Description = command.Request.Description ?? category.Description;
        category.ImageUrl = command.Request.ImageUrl ?? category.ImageUrl;
        category.IsHidden = command.Request.IsHidden ?? category.IsHidden;
        category.SortNumber = command.Request.SortNumber ?? category.SortNumber;
        category.UpdatedAt = DateTime.UtcNow;

        await _contentsContext.SaveChangesAsync(cancellationToken);

        return true;
    }

}

public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    public CategoryUpdateRequestValidator()
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