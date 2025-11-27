using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Application.Queries;
public interface IUserQueries
{
    Task<LoginUserDto> GetUserByPassword(string userAccount, string password);
    Task<LoginUserDto> GetUserById(int userId);

}
