using Microsoft.EntityFrameworkCore;
using Server.Common.Types;
using Server.Identity.Api.Infrastructure;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Application.Queries;

public class UserQueries(UserContext _userContext) : IUserQueries
{
    public async Task<UserDto> GetUserByPassword(string userAccount, string password)
    {
        var user = await _userContext.Users.FirstOrDefaultAsync(u => u.UserAccount == userAccount && u.UserPassword == password);
        if (user == null)
        {
            throw new ExceptionNotFound("User not found or password is incorrect");
        }
        return user.ToUserDto();
    }
    public async Task<UserDto> GetUserById(long userId)
    {
        var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new ExceptionNotFound($"User with ID {userId} not found");
        }
        return user.ToUserDto();
    }

    public async Task<List<UserDto>> GetUsersByIds(List<long> userIds)
    {
        var users = await _userContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        var userDtos = users.Select(u => u.ToUserDto()).ToList();
        return userDtos;
    }
}

