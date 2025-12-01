
namespace Server.Identity.Api.Models.Responses;

public class LoginUserDto
{
    public UserDto User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
}