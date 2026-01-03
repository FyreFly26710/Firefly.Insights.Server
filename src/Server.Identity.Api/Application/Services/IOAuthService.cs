using System;
using Server.Identity.Api.Models.Types;

namespace Server.Identity.Api.Application.Services;

public interface IOAuthService
{
    Task<TokenResponse> GetGmailToken(string code, string apiUrl);
    Task<UserInfo> GetUserInfoFromGmailToken(string gmailToken);
}
