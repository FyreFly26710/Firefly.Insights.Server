namespace Server.Contents.Api.Application.Commands;

public record TopicCreateCommand(TopicCreateRequest Request) : IRequest<long?>;
public class TopicCreateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<TopicCreateCommand, long?>
{
    public async Task<long?> Handle(TopicCreateCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
        if (category is null)
            throw new ExceptionNotFound($"Category of id {command.Request.CategoryId} not found");

        var maxSortNumber = await _contentsContext.Topics.MaxAsync(t => t.SortNumber, cancellationToken);
        var topic = new Topic()
        {
            Name = command.Request.Name,
            Description = command.Request.Description,
            CategoryId = command.Request.CategoryId,
            ImageUrl = command.Request.ImageUrl,
            SortNumber = maxSortNumber + 1,
        };
        await _contentsContext.Topics.AddAsync(topic, cancellationToken);
        await _contentsContext.SaveChangesAsync(cancellationToken);
        return topic.Id;
    }
}

public class TopicCreateRequestValidator : AbstractValidator<TopicCreateRequest>
{
    public TopicCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Topic name is required.")
            .MaximumLength(128).WithMessage("Topic name cannot exceed 128 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(256).WithMessage("Description cannot exceed 256 characters.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(256).WithMessage("Image URL cannot exceed 256 characters.")
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Image URL must be a valid URL.");
    }
}