
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

            if (!context.Users.Any())
            {
                var admin = new User() { Id = 1, UserName = "Firefly", UserAccount = "Firefly", UserPassword = "Password", UserRole = "admin", UserEmail = "lee.wan1204@gmail.com" };
                var user = new User() { Id = 2, UserName = "TestUser", UserAccount = "TestAccount", UserPassword = "Password", UserRole = "user" };
                var agents = GetAgents();
                await context.Users.AddRangeAsync(user, admin);
                await context.Users.AddRangeAsync(agents);
                await context.SaveChangesAsync();
            }
        }

        private static List<User> GetAgents() => [
            new() { UserName = "GPT-5.2", UserAccount = "gpt-5.2", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GPT_V2 },
            new() { UserName = "GPT-5", UserAccount = "gpt-5", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GPT_V2 },
            new() { UserName = "GPT-4.1", UserAccount = "gpt-4.1", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GPT_V1 },
            new() { UserName = "GPT-5-mini", UserAccount = "gpt-5-mini", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GPT_V1 },
            new() { UserName = "GPT-5-nano", UserAccount = "gpt-5-nano", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GPT_V1 },

            new() { UserName = "Gemini 3 Pro", UserAccount = "gemini-3-pro", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GEMINI_V2 },
            new() { UserName = "Gemini 2.5 Pro", UserAccount = "gemini-2.5-pro", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GEMINI_V2 },
            new() { UserName = "Gemini 3 Flash", UserAccount = "gemini-3-flash", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GEMINI_V1 },
            new() { UserName = "Gemini 2.5 Flash", UserAccount = "gemini-2.5-flash", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GEMINI_V1 },
            new() { UserName = "Gemini 2.5 Flash Lite", UserAccount = "gemini-2.5-flash-lite", UserPassword = "Password", UserRole = "agent", UserAvatar = AVATAR_GEMINI_V1 },
        ];

        private const string AVATAR_GPT_V1 = "https://static.vecteezy.com/system/resources/previews/021/608/790/non_2x/chatgpt-logo-chat-gpt-icon-on-black-background-free-vector.jpg";
        private const string AVATAR_GPT_V2 = "https://research.aimultiple.com/wp-content/uploads/2023/03/chatgpt.webp";
        private const string AVATAR_GEMINI_V1 = "https://registry.npmmirror.com/@lobehub/icons-static-png/latest/files/dark/gemini-color.png";
        private const string AVATAR_GEMINI_V2 = "https://images.seeklogo.com/logo-png/62/1/google-gemini-icon-logo-png_seeklogo-623016.png";
    }
}
