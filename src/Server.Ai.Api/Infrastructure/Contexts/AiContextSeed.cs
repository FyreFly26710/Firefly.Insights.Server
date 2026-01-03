using System;
using Npgsql;
using Server.Common.Extensions;

namespace Server.Ai.Api.Infrastructure.Contexts;

public class AiContextSeed : IDbSeeder<AiContext>
{
    private readonly IConfiguration _configuration;
    public AiContextSeed(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SeedAsync(AiContext context)
    {
        context.Database.OpenConnection();
        ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();
        if (await context.AiModels.AnyAsync())
        {
            return;
        }
        await context.AiModels.AddRangeAsync(AiModelsSeed.GetAiModels(_configuration));
        await context.SaveChangesAsync();
    }

}
