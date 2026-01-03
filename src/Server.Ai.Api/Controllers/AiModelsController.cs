using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Ai.Api.Application.Queries;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/ai-models")]
public class AiModelsController(IAiModelQueries _aiModelQueries, ILogger<AiModelsController> _logger,
    IMediator _mediator) : ControllerBase
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
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> Delete(long id)
    {
        var result = await _mediator.Send(new DeleteAiModelCommand(id));
        return Ok(result);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> Update(long id, [FromBody] UpdateAiModelRequest request)
    {
        var result = await _mediator.Send(new UpdateAiModelCommand(request, id));
        return Ok(result);
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> Create([FromBody] CreateAiModelRequest request)
    {
        var result = await _mediator.Send(new CreateAiModelCommand(request));
        return Ok(result);
    }
}
