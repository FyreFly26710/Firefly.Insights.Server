
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Server.Common.Extensions;
using Server.Identity.Api.Models.Entities;

namespace Server.Identity.Api.Infrastructure
{
    public class UserContextSeed : IDbSeeder<UserContext>
    {
        public async Task SeedAsync(UserContext context)
        {
            context.Database.OpenConnection();
            ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

            if(!context.Users.Any())
            {
                var user = new User() { Id = 1, UserName = "TestUser", UserAccount = "TestAccount", UserPassword="Passord",UserRole="user" };
                await context.Users.AddRangeAsync(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
