using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Contents.Api.Controllers;

[ApiController]
[Route("api/contents/topics")]
public class TopicController(
    ITopicQueries _topicQueries,
    IMediator _mediator,
    ILogger<TopicController> _logger) : ControllerBase
{
    [HttpGet("{topicId}")]
    public async Task<ActionResult<TopicDto>> GetById(long topicId)
    {
        var topic = await _topicQueries.GetTopicById(topicId);
        return Ok(topic);
    }
    [HttpGet]
    public async Task<ActionResult<List<TopicDto>>> GetList()
    {
        var topics = await _topicQueries.GetTopicList();
        return Ok(topics);
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<long?>> Create(TopicCreateRequest request)
    {
        var topicId = await _mediator.Send(new TopicCreateCommand(request));
        if (topicId is null)
            return BadRequest("Failed to create topic");
        return Ok(topicId);
    }
    [HttpPut("{topicId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> Update(long topicId, [FromBody] TopicUpdateRequest request)
    {
        request = request with { TopicId = topicId };
        var result = await _mediator.Send(new TopicUpdateCommand(request));
        return Ok(result);
    }
    [HttpDelete("{topicId}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<bool>> Delete(long topicId)
    {
        var result = await _mediator.Send(new TopicDeleteCommand(topicId));
        return Ok(result);
    }
}
