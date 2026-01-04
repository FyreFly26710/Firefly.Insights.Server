using System.Text;
using System.Text.Json;
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
    IOAuthService _oAuthService,
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
    public async Task<ActionResult<LoginUserDto>> Login([FromBody] UserLoginRequest request)
    {

        var user = await _userQueries.GetUserByPassword(request.UserAccount, request.UserPassword);
        var token = _jwtService.GenerateToken(user.UserId.ToString(), user.UserName ?? "", user.UserRole);

        return Ok(new LoginUserDto
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
    [HttpGet("signin-google")]
    public async Task<ActionResult<LoginUserDto>> SignInGoogle([FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code)) throw new ExceptionBadRequest("Authorization code is missing.");
        
        var decodedState = Encoding.UTF8.GetString(Convert.FromBase64String(state));
        var stateData = JsonSerializer.Deserialize<Dictionary<string, string>>(decodedState);
        string rawOrigin = stateData?["origin"] ?? "*";
        string origin = System.Net.WebUtility.UrlDecode(rawOrigin);
        string apiUrl = stateData?["apiUrl"] ?? "";

        var tokenResponse = await _oAuthService.GetGmailToken(code, apiUrl);
        if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            throw new ExceptionBadRequest("Failed to get access token from Google.");

        var userInfo = await _oAuthService.GetUserInfoFromGmailToken(tokenResponse.AccessToken);
        var user = await _userQueries.GetUserByEmail(userInfo.Email);
        var token = _jwtService.GenerateToken(user.UserId.ToString(), user.UserName ?? "", user.UserRole);

        string script = $@"
        <html>
        <body>
            <script>
                window.opener.postMessage({{ 
                    type: 'GOOGLE_AUTH_SUCCESS', 
                    token: '{token}' 
                }}, '{origin}');
                
                window.close();
            </script>
            <p>Authentication successful! Closing window...</p>
        </body>
        </html>";

        return Content(script, "text/html");
    }
}

