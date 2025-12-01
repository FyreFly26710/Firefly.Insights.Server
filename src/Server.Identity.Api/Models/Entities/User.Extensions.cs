using System;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Models.Entities;

public partial class User
{
    public UserDto ToUserDto()
    {
        return new UserDto
        {
            Id = Id,
            UserAccount = UserAccount,
            UserName = UserName,
            UserEmail = UserEmail,
            UserAvatar = UserAvatar,
            UserProfile = UserProfile,
            UserRole = UserRole,
            CreateTime = CreatedAt,
            UpdateTime = UpdatedAt,
        };
    }
    public LoginUserDto ToLoginUserDto()
    {
        return new LoginUserDto
        {
            Id = Id,
            UserAccount = UserAccount,
            UserName = UserName,
            UserEmail = UserEmail,
            UserAvatar = UserAvatar,
            UserProfile = UserProfile,
            UserRole = UserRole,
            CreatedAt = CreatedAt,
        };
    }

}
