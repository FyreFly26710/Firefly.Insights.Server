using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Application.Queries;
public interface IUserQueries
{
    Task<UserDto> GetUserByPassword(string userAccount, string password);
    Task<UserDto> GetUserById(long userId);
    Task<UserDto> GetUserByEmail(string email);
    // Task<List<UserDto>> GetUsersByIds(List<long> userIds);

}
