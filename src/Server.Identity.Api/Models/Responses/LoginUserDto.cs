using Server.Identity.Api.Models.Entities;

namespace Server.Identity.Api.Models.Responses;
public class LoginUserDto
{

    public long Id { get; set; }
    public string UserAccount { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public string? UserAvatar { get; set; }
    public string? UserProfile { get; set; }
    public string UserRole { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }

    public LoginUserDto() { }
    public LoginUserDto(User user)
    {
        Id = user.Id;
        UserAccount = user.UserAccount;
        UserName = user.UserName;
        UserEmail = user.UserEmail;
        UserAvatar = user.UserAvatar;
        UserProfile = user.UserProfile;
        UserRole = user.UserRole;
        CreatedAt = user.CreatedAt;
    }
}
public class LoginUserWithTokenDto
{
    public LoginUserDto User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
}