using System;
using Npgsql;
using Server.Common.Extensions;

namespace Server.Ai.Api.Infrastructure.Contexts;

public class AiContextSeed : IDbSeeder<AiContext>
{
    private readonly IConfiguration _configuration;
    private readonly IMessageBus _messageBus;
    public AiContextSeed(IConfiguration configuration, IMessageBus messageBus)
    {
        _configuration = configuration;
        _messageBus = messageBus;
    }
    public async Task SeedAsync(AiContext context)
    {
        context.Database.OpenConnection();
        ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();
        if (await context.AiModels.AnyAsync())
        {
            return;
        }
        var models = AiModelsSeed.GetAiModels(_configuration);

        var users = new List<UserTo>();
        foreach (var model in models)
        {
            users.Add(new UserTo(model.Id, model.DisplayName, model.Avatar, "agent", model.Id.ToString()));
        }
        await _messageBus.PublishAsync(new CreateUsersMessage(users));
        context.AiModels.AddRange(models);
        await context.SaveChangesAsync();
    }

}
