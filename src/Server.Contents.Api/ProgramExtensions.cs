using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Server.Common.Extensions;
using Server.Common.Utils;
using Server.Contents.Api.Application.Queries;
using Server.Contents.Api.Infrastructure;
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

        services.AddScoped<IArticleQueries, ArticleQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<ITopicQueries, TopicQueries>();

        return services;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
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

