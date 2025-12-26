using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Server.Common.Extensions;
using Server.Common.Utils;
using Server.Contents.Api.Application.Behaviours;
using Server.Contents.Api.Application.Queries;
using Server.Contents.Api.Infrastructure.EfContexts;
using Server.Contents.Api.Infrastructure.RedisRepositories;
using StackExchange.Redis;
namespace Server.Contents.Api;
public static class ProgramExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        services.AddFluentValidationAutoValidation();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarker));
        });

        // this line seems to be unnecessary, because the validation is done at controller level
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        services.AddScoped<IArticleQueries, ArticleQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<ITopicQueries, TopicQueries>();

        return services;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ContentDb") ?? throw new Exception("ContentDb connection string is not configured");
        services.AddDbContext<ContentsContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        var redisConnectionString = configuration.GetConnectionString("Redis") ?? throw new Exception("Redis connection string is not configured");
        services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString));
        services.AddSingleton<ITagRepository, RedisTagRepository>();

        if (EnvUtil.IsDevelopment())
        {
            // seed static tags into Redis
            SeedRedisStaticTags(services);
            services.AddMigration<ContentsContext, ContentsContextSeed>();
        }
        return services;
    }

    private static void SeedRedisStaticTags(IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        var redis = provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase();

        foreach (var tag in TagTypeExtensions.GetStaticTags())
        {
            redis.HashSet($"tags:name:{tag.Type}", tag.Name, tag.Id, When.NotExists);
        }
    }


}

