namespace Server.Identity.Api.Interfaces.Services;
public interface IJwtService
{
    string GenerateToken(string userId, string username);
    string? GetUserId(string token);
}

