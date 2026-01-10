using FluentValidation.AspNetCore;
using Server.Ai.Api.Infrastructure.AiClients;
using Server.Ai.Api.Infrastructure.Messaging;
using Server.Ai.Api.Infrastructure.StateMachines;
using Server.Common.Behaviours;
using Server.Common.Extensions;
using Server.Common.Utils;
using Server.Messages.Contents;

namespace Server.Ai.Api;
public static class ProgramExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        services.AddFluentValidationAutoValidation();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarker));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        services.AddScoped<IJobLogQueries, JobLogQueries>();
        services.AddScoped<IExecutionLogQueries, ExecutionLogQueries>();
        services.AddScoped<IExecutionPayloadQueries, ExecutionPayloadQueries>();
        services.AddScoped<IAiModelQueries, AiModelQueries>();
        services.AddScoped<IAiProviderQueries, AiProviderQueries>();

        return services;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Services
        services.AddScoped<IArticleGenerationClient, ArticleGenerationClient>();

        // Message Bus
        services.AddScoped<IMessageBus, MassTransitMessageBus>();
        services.AddMassTransit(x =>
        {
            // Add consumers
            x.AddConsumer<GenerateArticleSummaryConsumer>();
            x.AddConsumer<GenerateArticleContentConsumer>();
            x.AddConsumer<GenerateTopicSummaryConsumer>();
            x.AddRequestClient<CreateTopicRequestMessage>();
            x.AddRequestClient<CreateArticleRequestMessage>();
            x.AddRequestClient<GetTopicRequestMessage>();
            x.AddRequestClient<UserRequestMessage>();
            x.AddRequestClient<UserListRequestMessage>();

            // Add saga state machine
            x.AddSagaStateMachine<ArticleGenerationSaga, ArticleGenerationSagaState>()
                .InMemoryRepository();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"], h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
                // cfg.UseMessageRetry(r => { r.Interval(3, TimeSpan.FromMinutes(10)); });
                cfg.ConfigureEndpoints(context);
            });
        });

        // Database
        var connectionString = configuration.GetConnectionString("AiDb");
        services.AddDbContext<AiContext>(options => { options.UseNpgsql(connectionString); });
        if (EnvUtil.IsDevelopment())
            services.AddMigration<AiContext, AiContextSeed>();

        return services;
    }


}

