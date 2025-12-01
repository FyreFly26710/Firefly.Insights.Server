using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Identity.Api.Application.Queries;
using Server.Identity.Api.Application.Services;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Controllers;

[ApiController]
[Route("api/identity/users")]
public class UserController(
    IUserQueries _userQueries,
    IMediator _mediator,
    ILogger<UserController> _logger)
    : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUserById(int userId)
    {
        var user = await _userQueries.GetUserById(userId);
        return Ok(user);
    }


}
