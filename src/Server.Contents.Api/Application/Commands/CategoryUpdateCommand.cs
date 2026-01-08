namespace Server.Contents.Api.Application.Commands;
public record CategoryUpdateCommand(CategoryUpdateRequest Request) : IRequest<bool>;
public class CategoryUpdateCommandHandler(ContentsContext _contentsContext) : IRequestHandler<CategoryUpdateCommand, bool>
{
    public async Task<bool> Handle(CategoryUpdateCommand command, CancellationToken cancellationToken)
    {
        var category = await _contentsContext.Categories.FindAsync(command.Request.CategoryId, cancellationToken);
        if (category is null)
            throw new ExceptionNotFound($"Category of id {command.Request.CategoryId} not found");
        if (command.Request.Topics is not null)
        {
            // remove all existing topics
            var existingTopics = await _contentsContext.Topics.Where(x => x.CategoryId == category.Id).ToListAsync(cancellationToken);
            foreach (var existingTopic in existingTopics)
            {
                existingTopic.CategoryId = -1L;
                existingTopic.UpdatedAt = DateTime.UtcNow;
            }
            // add new topics
            var topicIds = command.Request.Topics.Select(x => x.TopicId).ToList();
            var topics = await _contentsContext.Topics.Where(x => topicIds.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var topic in topics)
            {
                var topicRequest = command.Request.Topics.First(x => x.TopicId == topic.Id);
                topic.CategoryId = category.Id;
                topic.SortNumber = topicRequest.SortNumber;
                topic.IsHidden = topicRequest.IsHidden;
                topic.UpdatedAt = DateTime.UtcNow;
            }
        }

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
            .When(x => x.Name is not null);

    }
}