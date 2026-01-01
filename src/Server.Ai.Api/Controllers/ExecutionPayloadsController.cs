using System;
using Microsoft.AspNetCore.Mvc;
using Server.Ai.Api.Application.Queries;
using Server.Ai.Api.Models.Responses;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/execution-payloads")]
public class ExecutionPayloadsController(IExecutionPayloadQueries _executionPayloadQueries, ILogger<ExecutionPayloadsController> _logger) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ExecutionPayloadDto>> GetById(long id)
    {
        var executionPayload = await _executionPayloadQueries.GetByIdAsync(id);
        return Ok(executionPayload);
    }
}
