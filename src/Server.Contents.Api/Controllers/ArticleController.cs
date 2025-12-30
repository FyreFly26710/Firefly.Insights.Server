using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Server.Contents.Api.Controllers;

[ApiController]
[Route("api/contents/articles")]
public class ArticleController(
    IArticleQueries _articleQueries,
    IMediator _mediator,
    ILogger<ArticleController> _logger) : ControllerBase
{
    [HttpGet("{articleId}")]
    public async Task<ActionResult<ArticleDto>> GetById(long articleId)
    {
        var article = await _articleQueries.GetArticleById(articleId);
        return Ok(article);
    }
    [HttpGet]
    public async Task<ActionResult<Paged<ArticleDto>>> GetList([FromQuery] ArticleListRequest request)
    {
        var articles = await _articleQueries.GetArticleList(request);
        return Ok(articles);
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<long?>> Create([FromBody] ArticleCreateRequest request)
    {
        var articleId = await _mediator.Send(new ArticleCreateCommand(request, 1));
        return Ok(articleId);
    }
    [HttpPut("{articleId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> Update(long articleId, [FromBody] ArticleUpdateRequest request)
    {
        request = request with { ArticleId = articleId };
        var result = await _mediator.Send(new ArticleUpdateCommand(request));
        return Ok(result);
    }
    [HttpDelete("{articleId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> Delete(long articleId)
    {
        var result = await _mediator.Send(new ArticleDeleteCommand(articleId));
        return Ok(result);
    }
}
