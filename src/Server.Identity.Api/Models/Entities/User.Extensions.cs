using System;
using Server.Identity.Api.Models.Responses;

namespace Server.Identity.Api.Models.Entities;

public partial class User
{
    public UserDto ToUserDto()
    {
        return new UserDto
        {
            UserId = Id,
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

}
