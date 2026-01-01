using System;
using Server.Ai.Api.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/execution-logs")]
public class ExecutionLogsController
(IExecutionLogQueries _executionLogQueries, ILogger<ExecutionLogsController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Paged<ExecutionLogDto>>> GetList([FromQuery] ExecutionLogListRequest request)
    {
        var executionLogs = await _executionLogQueries.GetListAsync(request);
        return Ok(executionLogs);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ExecutionLogDto>> GetById(long id)
    {
        var executionLog = await _executionLogQueries.GetByIdAsync(id);
        return Ok(executionLog);
    }
}
