using Microsoft.EntityFrameworkCore;
using Server.Common.Types;
using Server.Identity.Api.Infrastructure;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Application.Queries;

public class UserQueries(UserContext _userContext) : IUserQueries
{
    public async Task<LoginUserDto> GetUserByPassword(string userAccount, string password)
    {
        var user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserAccount == userAccount && u.UserPassword == password);
        if (user == null)
        {
            throw new ExceptionNotFound("User not found or password is incorrect");
        }
        return user.ToLoginUserDto();
    }
    public async Task<LoginUserDto> GetUserById(int userId)
    {
        var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new ExceptionNotFound($"User with ID {userId} not found");
        }
        return user.ToLoginUserDto();
    }
}

