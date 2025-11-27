namespace Server.Identity.Api.Models.Responses;
public class LoginUserDto
{

    public long Id { get; set; }
    public string UserAccount { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public string? UserAvatar { get; set; }
    public string? UserProfile { get; set; }
    public string UserRole { get; set; }
    public DateTime? CreateTime { get; set; }
    //public DateTime UpdateTime { get; set; }
}
public class LoginUserWithTokenDto 
{
    public LoginUserDto User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
}