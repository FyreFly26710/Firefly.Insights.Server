using System;
using Microsoft.AspNetCore.Mvc;
using Server.Ai.Api.Application.Queries;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/ai-models")]
public class AiModelsController(IAiModelQueries _aiModelQueries, ILogger<AiModelsController> _logger
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AiModelDto>>> GetList([FromQuery] AiModelListRequest request)
    {
        var aiModels = await _aiModelQueries.GetListAsync(request);
        return Ok(aiModels);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<AiModelDto>> GetById(long id)
    {
        var aiModel = await _aiModelQueries.GetByIdAsync(id);
        return Ok(aiModel);
    }
    [HttpGet("lookup-list")]
    public async Task<ActionResult<List<LookupItemDto>>> GetLookupList()
    {
        var lookupList = await _aiModelQueries.GetLookupList();
        return Ok(lookupList);
    }
}
