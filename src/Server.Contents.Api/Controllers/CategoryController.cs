using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Contents.Api.Application.Queries;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Controllers;

[ApiController]
[Route("api/contents/categories")]
public class CategoryController(
    ICategoryQueries _categoryQueries,
    IMediator _mediator,
    ILogger<CategoryController> _logger) : ControllerBase
{
    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(long categoryId)
    {
        var category = await _categoryQueries.GetCategoryById(categoryId);
        return Ok(category);
    }
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategoryList()
    {
        var categories = await _categoryQueries.GetCategoryList();
        return Ok(categories);
    }
}
