namespace Server.Contents.Api.Application.Queries;

public interface ICategoryQueries
{
    Task<CategoryDto> GetCategoryById(long categoryId);
    Task<List<CategoryDto>> GetCategoryList();
    Task<List<LookupItemDto>> GetLookupList();
    Task<List<LookupItemDto>> GetTopicLookupList(long categoryId);
}
