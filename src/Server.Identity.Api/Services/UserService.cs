using Server.Identity.Api.Interfaces.Services;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Services;
public class UserService() : IUserService
{
    //public async Task<LoginUserDto> GetLoginUser(string jwtToken)
    //{
    //    var user = new LoginUserDto() {Id = 1, UserName="test" };

    //    return user;
    //}

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
