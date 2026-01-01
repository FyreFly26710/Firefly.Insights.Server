using System;
using Microsoft.AspNetCore.Mvc;
using Server.Ai.Api.Application.Queries;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/job-logs")]
public class JobLogsController(IJobLogQueries _jobLogQueries, ILogger<JobLogsController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Paged<JobLogDto>>> GetList([FromQuery] JobLogListRequest request)
    {
        var jobLogs = await _jobLogQueries.GetJobLogsAsync(request);
        return Ok(jobLogs);
    }
    [HttpGet("{id}/execution-log")]
    public async Task<ActionResult<ExecutionLogDto>> GetExecutionLogById(long id)
    {
        var executionLog = await _jobLogQueries.GetExecutionLogByIdAsync(id);
        return Ok(executionLog);
    }
}
