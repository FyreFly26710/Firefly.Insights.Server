using Microsoft.AspNetCore.Mvc;
using Server.Ai.Api.Models.Requests;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/article-generation")]
public class ArticleGenerationController(
    IAiClient _aiClient,
    ILogger<ArticleGenerationController> _logger) : ControllerBase
{
    [HttpPost("article-summary")]
    public async Task<ActionResult<string>> GenerateArticleSummary([FromBody] GenerateArticleSummaryRequest request)
    {
        var result = await _aiClient.GenerateArticleSummaryList(request);
        return Ok(result);
    }
}