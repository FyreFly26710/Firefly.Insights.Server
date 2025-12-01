using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Types;
using Server.Contents.Api.Application.Queries;
using Server.Contents.Api.Models.Requests;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Controllers;

[ApiController]
[Route("api/contents/articles")]
public class ArticleController(
    IArticleQueries _articleQueries,
    IMediator _mediator,
    ILogger<ArticleController> _logger) : ControllerBase
{
    [HttpGet("{articleId}")]
    public async Task<ActionResult<ArticleDto>> GetArticleById(long articleId)
    {
        var article = await _articleQueries.GetArticleById(articleId);
        return Ok(article);
    }
    [HttpGet]
    public async Task<ActionResult<Paged<ArticleDto>>> GetArticleList([FromQuery] ArticleListRequest request)
    {
        var articles = await _articleQueries.GetArticleList(request);
        return Ok(articles);
    }
}
