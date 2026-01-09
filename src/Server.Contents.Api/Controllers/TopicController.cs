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
    [HttpGet("lookup-list")]
    public async Task<ActionResult<List<LookupItemDto>>> GetLookupList()
    {
        var lookupList = await _topicQueries.GetLookupList();
        return Ok(lookupList);
    }
    [HttpGet]
    public async Task<ActionResult<Paged<TopicDto>>> GetList([FromQuery] TopicListRequest request)
    {
        var topics = await _topicQueries.GetTopicList(request);
        return Ok(topics);
    }
    [HttpGet("{topicId}/summary-article-id")]
    public async Task<ActionResult<long>> GetSummaryArticleId(long topicId)
    {
        var articleId = await _topicQueries.GetSummaryArticleId(topicId);
        return Ok(articleId);
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
