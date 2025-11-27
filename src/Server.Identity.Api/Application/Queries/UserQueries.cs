using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Application.Queries;

public class UserQueries : IUserQueries
{
    public async Task<LoginUserDto> GetUserByPassword(string userAccount, string password)
    {
        var user = new LoginUserDto()
        {
            Id = 1,
            UserName = "TestUser",
        };
        return user;
    }
    public async Task<LoginUserDto> GetUserById(int userId)
    {
        var user = new LoginUserDto()
        {
            Id = 1,
            UserName = "TestUser",
        };
        return user;
    }
}

