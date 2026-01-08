
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Server.Common.Extensions;
using Server.Identity.Api.Models.Constants;
using Server.Identity.Api.Models.Entities;

namespace Server.Identity.Api.Infrastructure
{
    public class UserContextSeed : IDbSeeder<UserContext>
    {
        private bool _isSeeded = false;
        public async Task SeedAsync(UserContext context)
        {
            context.Database.OpenConnection();
            ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

            if (await context.Users.FindAsync(1L) is null)
                await context.Users.AddAsync(Firefly());

            if (await context.Users.FindAsync(2L) is null)
                await context.Users.AddAsync(TestAdmin());

            if (await context.Users.FindAsync(3L) is null)
                await context.Users.AddAsync(TestUser());

            if (_isSeeded)
                await context.SaveChangesAsync();
        }

        private User Firefly()
        {
            _isSeeded = true;
            return new User() { Id = 1L, UserName = "Firefly", UserAccount = "Firefly", UserPassword = "", UserRole = "admin", UserEmail = "lee.wan1204@gmail.com" };
        }
        private User TestAdmin()
        {
            _isSeeded = true;
            return new User() { Id = 2L, UserName = "TestAdmin", UserAccount = "TestAdmin", UserPassword = Passwords.DefaultPassword, UserRole = "admin" };
        }
        private User TestUser()
        {
            _isSeeded = true;
            return new User() { Id = 3L, UserName = "TestUser", UserAccount = "TestUser", UserPassword = Passwords.DefaultPassword, UserRole = "user" };
        }
    }


}
