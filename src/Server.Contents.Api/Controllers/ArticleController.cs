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
    public async Task<ActionResult<long?>> Create([FromBody] ArticleCreateRequest request)
    {
        var articleId = await _mediator.Send(new ArticleCreateCommand(request));
        return Ok(articleId);
    }
    [HttpPut("{articleId}")]
    public async Task<ActionResult<bool>> Update([FromBody] ArticleUpdateRequest request)
    {
        var result = await _mediator.Send(new ArticleUpdateCommand(request));
        return Ok(result);
    }
    [HttpDelete("{articleId}")]
    public async Task<ActionResult<bool>> Delete(long articleId)
    {
        var result = await _mediator.Send(new ArticleDeleteCommand(articleId));
        return Ok(result);
    }
}
