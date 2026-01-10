using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Server.Common.Configurations;
using Server.Common.Types;

namespace Server.Identity.Api.Application.Services;
public class JwtService(IOptions<JwtSettings> _jwtSettings, ILogger<JwtService> _logger) : IJwtService
{

    public string GenerateToken(string userId, string username, string role)
    {
        var settings = _jwtSettings.Value;
        var key = Encoding.UTF8.GetBytes(settings.Key);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            //issuer: jwtSettings["Issuer"],
            //audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(settings.ExpireHours)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string? GetUserId(string token)
    {
        if (string.IsNullOrEmpty(token)) throw new ExceptionBadRequest("Token is required");

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        return userIdClaim?.Value;

    }
}
