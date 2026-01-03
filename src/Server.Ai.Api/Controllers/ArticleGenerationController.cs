using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Ai.Api.Models.Requests;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/article-generations")]
public class ArticleGenerationController(
    IMediator _mediator,
    ILogger<ArticleGenerationController> _logger) : ControllerBase
{
    [HttpPost("article-summary")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> GenerateArticleSummary([FromBody] GenerateArticleSummaryRequest request)
    {
        var command = new GenerateArticleSummaryCommand(request);
        var result = await _mediator.Send(command);
        return Ok(true);
    }
}