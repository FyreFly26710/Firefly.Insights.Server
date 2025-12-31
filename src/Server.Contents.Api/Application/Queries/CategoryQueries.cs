namespace Server.Contents.Api.Application.Queries;

public class CategoryQueries(ContentsContext _contentsContext, ILogger<CategoryQueries> _logger) : ICategoryQueries
{
    private IQueryable<Category> GetNavigationQuery()
    {
        IQueryable<Category> query = _contentsContext.Categories.AsQueryable().AsNoTracking()
            .Include(c => c.Topics);
        return query;
    }
    public async Task<CategoryDto> GetCategoryById(long categoryId)
    {
        var query = GetNavigationQuery();
        var category = await query.FirstOrDefaultAsync(c => c.Id == categoryId);
        if (category is null)
            throw new ExceptionNotFound();
        category.Topics = category.Topics.OrderBy(t => t.SortNumber).ToList();
        return category.ToCategoryDto();
    }

    public async Task<List<CategoryDto>> GetCategoryList()
    {
        var query = GetNavigationQuery();
        var categories = await query.ToListAsync();
        categories = categories.OrderBy(c => c.SortNumber).ToList();
        return categories.Select(c => c.ToCategoryDto()).ToList();
    }
    public async Task<List<LookupItemDto>> GetLookupList()
    {
        var query = _contentsContext.Categories.AsQueryable().AsNoTracking();
        var categories = await query.OrderBy(c => c.SortNumber).Select(c => new LookupItemDto(c.Id, c.Name)).ToListAsync();
        return categories;
    }
    public async Task<List<LookupItemDto>> GetTopicLookupList(long categoryId)
    {
        var query = GetNavigationQuery();
        var category = await query.FirstOrDefaultAsync(c => c.Id == categoryId);
        if (category is null)
            throw new ExceptionNotFound();
        var topics = category.Topics.OrderBy(t => t.SortNumber).Select(t => new LookupItemDto(t.Id, t.Name)).ToList();
        return topics;
    }
}
