using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Types;
using Server.Common.Utils;
using Server.Identity.Api.Application.Commands;
using Server.Identity.Api.Application.Queries;
using Server.Identity.Api.Application.Services;
using Server.Identity.Api.Models.Requests;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Controllers;

[ApiController]
[Route("api/identity/auth")]
public class AuthController(
    IUserQueries _userQueries,
    IJwtService _jwtService,
    IMediator _mediator,
    ILogger<AuthController> _logger)
    : ControllerBase
{

    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register([FromBody] UserRegisterRequest request)
    {
        if (EnvUtil.IsProduction())
            throw new ExceptionForbidden("User register is disabled in production environment");

        var command = new RegisterUserCommand(request.UserAccount, request.UserPassword);
        bool result = await _mediator.Send(command);

        return Ok(result);
    }
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserWithTokenDto>> Login([FromBody] UserLoginRequest request)
    {

        var user = await _userQueries.GetUserByPassword(request.UserAccount, request.UserPassword);
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName ?? "");

        return Ok(new LoginUserWithTokenDto
        {
            User = user,
            Token = token
        });
    }

    [HttpPost("getLoginUser")]
    [Authorize]
    public async Task<ActionResult<LoginUserDto>> GetLoginUser()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        var jwtToken = authHeader!.Substring("Bearer ".Length).Trim();

        var userId = _jwtService.GetUserId(jwtToken);
        if (int.TryParse(userId, out int userIdInt))
        {
            var user = await _userQueries.GetUserById(userIdInt);
            return Ok(user);
        }
        else
        {
            throw new ExceptionBadRequest("Invalid user ID in token.");
        }
    }
    //[HttpGet("signin-google")]
    //public async Task<IActionResult> SignInGoogle([FromQuery] string code)
    //{
    //    if (string.IsNullOrEmpty(code)) throw new ApiException(ErrorCode.PARAMS_ERROR, "Authorization code is missing.");

    //    var tokenResponse = await _oAuthService.GetGmailToken(code);
    //    if (string.IsNullOrEmpty(tokenResponse.AccessToken))
    //        throw new ApiException(ErrorCode.OPERATION_ERROR, "Failed to get access token from Google.");

    //    var userInfo = await _oAuthService.GetUserInfoFromGmailToken(tokenResponse.AccessToken);
    //    if (string.IsNullOrEmpty(userInfo.Email))
    //        throw new ApiException(ErrorCode.OPERATION_ERROR, "Failed to get user email from Google.");

    //    var user = await _userService.GetUserByEmail(userInfo.Email);
    //    if (user == null)
    //        throw new ApiException(ErrorCode.OPERATION_ERROR, "User not found with provided email.");

    //    await _userService.SignInUser(user, HttpContext);

    //    // Redirect to home
    //    string? homePage = _config["Domain:Home"];
    //    return Redirect(homePage ?? "/");
    //}
}

