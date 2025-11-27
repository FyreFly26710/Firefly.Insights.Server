using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Server.Common.Configurations;

namespace Server.Identity.Api.Application.Services;
public class JwtService(IOptions<JwtSettings> _jwtSettings) : IJwtService
{

    public string GenerateToken(string userId, string username)
    {
        var settings = _jwtSettings.Value;
        var key = Encoding.UTF8.GetBytes(settings.Key);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username)
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
        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken;

        try
        {
            jwtToken = handler.ReadJwtToken(token);
        }
        catch
        {
            return null; // Invalid token
        }

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        return userIdClaim?.Value;
    }
}
