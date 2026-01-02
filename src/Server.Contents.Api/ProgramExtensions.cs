using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Server.Common.Extensions;
using Server.Common.Messaging;
using Server.Common.Utils;
using Server.Contents.Api.Application.Behaviours;
using Server.Contents.Api.Application.Queries;
using Server.Contents.Api.Infrastructure;
using Server.Contents.Api.Infrastructure.Messaging;
using Server.Messages.Contents;
using Server.Messages.Identities;
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
        services.AddScoped<IMessageBus, MassTransitMessageBus>();
        services.AddMassTransit(x =>
        {
            // Add consumers
            x.AddConsumer<CreateTopicRequestConsumer>();
            x.AddConsumer<GetTopicRequestConsumer>();
            x.AddConsumer<CreateArticleRequestConsumer>();

            x.AddRequestClient<UserListRequestMessage>();
            x.AddRequestClient<UserRequestMessage>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"], h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });



        var connectionString = configuration.GetConnectionString("ContentDb");
        services.AddDbContext<ContentsContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        if (EnvUtil.IsDevelopment())
        {
            services.AddMigration<ContentsContext, ContentsContextSeed>();
        }


        return services;
    }


}

