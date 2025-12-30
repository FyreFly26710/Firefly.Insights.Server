namespace Server.Identity.Api.Application.Services;
public interface IJwtService
{
    string GenerateToken(string userId, string username, string role);
    string? GetUserId(string token);
}

