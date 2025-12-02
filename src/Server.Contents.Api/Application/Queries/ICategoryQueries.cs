using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Application.Queries;

public interface ICategoryQueries
{
    Task<CategoryDto> GetCategoryById(long categoryId);
    Task<List<CategoryDto>> GetCategoryList();
}
