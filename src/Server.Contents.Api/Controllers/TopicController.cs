using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Contents.Api.Application.Queries;
using Server.Contents.Api.Models.Responses;

namespace Server.Contents.Api.Controllers;

[ApiController]
[Route("api/contents/topics")]
public class TopicController(
    ITopicQueries _topicQueries,
    IMediator _mediator,
    ILogger<TopicController> _logger) : ControllerBase
{
    [HttpGet("{topicId}")]
    public async Task<ActionResult<TopicDto>> GetTopicById(long topicId)
    {
        var topic = await _topicQueries.GetTopicById(topicId);
        return Ok(topic);
    }
    [HttpGet]
    public async Task<ActionResult<List<TopicDto>>> GetTopicList()
    {
        var topics = await _topicQueries.GetTopicList();
        return Ok(topics);
    }
}
