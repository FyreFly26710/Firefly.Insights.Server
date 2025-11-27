using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Interfaces.Services;
public interface IUserService
{
    Task<LoginUserDto> GetUserByPassword(string userAccount, string password);
    Task<LoginUserDto> GetUserById(int userId);
}


