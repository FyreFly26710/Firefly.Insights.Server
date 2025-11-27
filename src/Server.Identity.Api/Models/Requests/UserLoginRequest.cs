namespace Server.Identity.Api.Models.Requests;
public class UserLoginRequest
{
    public String UserAccount { get; set; }
    public String UserPassword { get; set; }
}