using System;
using Microsoft.AspNetCore.Mvc;

namespace Server.Ai.Api.Controllers;

[ApiController]
[Route("api/ai/ai-providers")]
public class AiProvidersController(IAiProviderQueries _aiProviderQueries, ILogger<AiProvidersController> _logger,
    IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var providers = await _aiProviderQueries.GetListAsync();
        return Ok(providers);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateAiProviderRequest request)
    {
        var result = await _mediator.Send(new UpdateAiProviderCommand(request, id));
        return Ok(result);
    }
}
