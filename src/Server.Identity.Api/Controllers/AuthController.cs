using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Exceptions;
using Server.Identity.Api.Interfaces.Services;
using Server.Identity.Api.Models.Requests;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Controllers;

[ApiController]
[Route("api/identity/auth")]
public class AuthController(
    IUserService _userService,
    IJwtService _jwtService,
    ILogger<AuthController> _logger)
    : ControllerBase
{

    //[HttpPost("register")]
    //public async Task<ActionResult<string>> Register([FromBody] UserRegisterRequest request)
    //{
    //    if (EnvUtil.IsProduction())
    //        throw new ForbiddenException("User register is disabled in production environment");


    //    long result = await _authService.UserRegister(request.UserAccount, request.UserPassword, request.ConfirmPassword);

    //    return Ok(result.ToString());
    //}
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserWithTokenDto>> Login([FromBody] UserLoginRequest request)
    {

        var user = await _userService.GetUserByPassword(request.UserAccount, request.UserPassword);
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName??"");

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
        if(int.TryParse(userId, out int userIdInt))
        {
            var user = await _userService.GetUserById(userIdInt);
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