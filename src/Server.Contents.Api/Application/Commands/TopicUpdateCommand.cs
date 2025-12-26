using Server.Contents.Api.Infrastructure.EfContexts;

namespace Server.Contents.Api.Application.Commands;

public record TopicUpdateCommand(TopicUpdateRequest Request) : IRequest<bool>;
public class TopicUpdateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<TopicUpdateCommand, bool>
{
    public async Task<bool> Handle(TopicUpdateCommand command, CancellationToken cancellationToken)
    {
        var topic = await _contentsContext.Topics.FindAsync(command.Request.TopicId, cancellationToken);
        if (topic is null)
            throw new ExceptionNotFound($"Topic of id {command.Request.TopicId} not found");
            
        if (command.Request.CategoryId is not null)
        {
            var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
            if (category is null)
                throw new ExceptionNotFound($"Category of id {command.Request.CategoryId} not found");
        }

        topic.Name = command.Request.Name ?? topic.Name;
        topic.Description = command.Request.Description ?? topic.Description;
        topic.CategoryId = command.Request.CategoryId ?? topic.CategoryId;
        topic.ImageUrl = command.Request.ImageUrl ?? topic.ImageUrl;
        topic.SortNumber = command.Request.SortNumber ?? topic.SortNumber;
        topic.IsHidden = command.Request.IsHidden ?? topic.IsHidden;
        topic.UpdatedAt = DateTime.UtcNow;

        await _contentsContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
public class TopicUpdateRequestValidator : AbstractValidator<TopicUpdateRequest>
{
    public TopicUpdateRequestValidator()
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