using Microsoft.AspNetCore.Mvc;

namespace Server.Contents.Api.Controllers;

[ApiController]
[Route("api/contents/categories")]
public class CategoryController(
    ICategoryQueries _categoryQueries,
    IMediator _mediator,
    ILogger<CategoryController> _logger) : ControllerBase
{
    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> GetById(long categoryId)
    {
        var category = await _categoryQueries.GetCategoryById(categoryId);
        return Ok(category);
    }
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetList()
    {
        var categories = await _categoryQueries.GetCategoryList();
        return Ok(categories);
    }
    [HttpPost]
    public async Task<ActionResult<long?>> Create(CategoryCreateRequest request)
    {
        var categoryId = await _mediator.Send(new CategoryCreateCommand(request));
        if (categoryId is null)
            return BadRequest("Failed to create category");
        return Ok(categoryId);
    }
    [HttpPut("{categoryId}")]
    public async Task<ActionResult<bool>> Update(long categoryId, [FromBody] CategoryUpdateRequest request)
    {
        request = request with { CategoryId = categoryId };
        var result = await _mediator.Send(new CategoryUpdateCommand(request));
        return Ok(result);
    }
    [HttpDelete("{categoryId}")]
    public async Task<ActionResult<bool>> Delete(long categoryId)
    {
        var result = await _mediator.Send(new CategoryDeleteCommand(categoryId));
        if (!result)
            return BadRequest("Failed to delete category");
        return Ok(result);
    }
}
