using System;
using Microsoft.EntityFrameworkCore;
using Server.Common.Types;
using Server.Contents.Api.Infrastructure;
using Server.Contents.Api.Models.Entities;
using Server.Contents.Api.Models.Responses;

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
        return categories.Select(c => c.ToCategoryDto()).ToList();
    }
}
