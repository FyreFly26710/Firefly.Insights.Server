
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Server.Common.Extensions;
using Server.Contents.Api.Models.Entities;

namespace Server.Contents.Api.Infrastructure;

public class ContentsContextSeed : IDbSeeder<ContentsContext>
{
    public async Task SeedAsync(ContentsContext context)
    {
        context.Database.OpenConnection();
        ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

        if (await context.Tags.AnyAsync())
        {
            return;
        }
        await context.ArticleTags.ExecuteDeleteAsync();
        await context.ArticleMetas.ExecuteDeleteAsync();
        await context.Articles.ExecuteDeleteAsync();
        await context.Topics.ExecuteDeleteAsync();
        await context.Categories.ExecuteDeleteAsync();
        await context.Tags.ExecuteDeleteAsync();

        await context.Tags.AddRangeAsync(SeedData.Tags);
        await context.Articles.AddRangeAsync(SeedData.Articles);
        await context.Categories.AddRangeAsync(SeedData.Categories);
        await context.Topics.AddRangeAsync(SeedData.Topics);
        await context.ArticleMetas.AddRangeAsync(SeedData.ArticleMetas);
        await context.ArticleTags.AddRangeAsync(SeedData.ArticleTags);
        await context.SaveChangesAsync();

    }
}

public class SeedData
{
    public static List<Article> Articles = [
        new Article() { Id = 1, Title = "Article 1", Content = "Content 1"},
        new Article() { Id = 2, Title = "Article 2", Content = "Content 2"},
        new Article() { Id = 3, Title = "Article 3", Content = "Content 3"},
        new Article() { Id = 4, Title = "Article 4", Content = "Content 4"},
        new Article() { Id = 5, Title = "Article 5", Content = "Content 5"},
        new Article() { Id = 6, Title = "Article 6", Content = "Content 6"},
        new Article() { Id = 7, Title = "Article 7", Content = "Content 7"},
        new Article() { Id = 8, Title = "Article 8", Content = "Content 8"},
        new Article() { Id = 9, Title = "Article 9", Content = "Content 9"},
        new Article() { Id = 10, Title = "Article 10", Content = "Content 10"},
        new Article() { Id = 11, Title = "Article 11", Content = "Content 11"},
        new Article() { Id = 12, Title = "Article 12", Content = "Content 12"},
        new Article() { Id = 13, Title = "Article 13", Content = "Content 13"},
        new Article() { Id = 14, Title = "Article 14", Content = "Content 14"},
        new Article() { Id = 15, Title = "Article 15", Content = "Content 15"},
        new Article() { Id = 16, Title = "Article 16", Content = "Content 16"},
        new Article() { Id = 17, Title = "Article 17", Content = "Content 17"},
        new Article() { Id = 18, Title = "Article 18", Content = "Content 18"},
        new Article() { Id = 19, Title = "Article 19", Content = "Content 19"},
        new Article() { Id = 20, Title = "Article 20", Content = "Content 20"},
    ];
    public static List<Tag> Tags = [
        new Tag() { Id = 1, Name = "Beginner", Type = TagType.SkillLevel},
        new Tag() { Id = 2, Name = "Advanced", Type = TagType.SkillLevel},
        new Tag() { Id = 3, Name = "Expert", Type = TagType.SkillLevel},
        new Tag() { Id = 4, Name = "General", Type = TagType.SkillLevel},
        new Tag() { Id = 5, Name = "Overview", Type = TagType.ArticleStyle},
        new Tag() { Id = 6, Name = "Deep-dive", Type = TagType.ArticleStyle},
        new Tag() { Id = 7, Name = "Best-practices", Type = TagType.ArticleStyle},
        new Tag() { Id = 8, Name = "Listicle", Type = TagType.ArticleStyle},
        new Tag() { Id = 9, Name = "Q&A", Type = TagType.ArticleStyle},
        new Tag() { Id = 10, Name = "Comparison", Type = TagType.ArticleStyle},
        new Tag() { Id = 11, Name = "Conversational", Type = TagType.Tone},
        new Tag() { Id = 12, Name = "Academic", Type = TagType.Tone},
        new Tag() { Id = 13, Name = "Technical", Type = TagType.Tone},
        new Tag() { Id = 14, Name = "Code-heavy", Type = TagType.Tone},
        new Tag() { Id = 15, Name = "Performance Optimization", Type = TagType.FocusArea},
        new Tag() { Id = 16, Name = "C#", Type = TagType.TechStack},
    ];
    public static List<ArticleTag> ArticleTags = [
        new ArticleTag() { Id = 1, ArticleMetaId = 1, TagId = 1},
        new ArticleTag() { Id = 2, ArticleMetaId = 1, TagId = 5},
        new ArticleTag() { Id = 3, ArticleMetaId = 1, TagId = 11},
        new ArticleTag() { Id = 4, ArticleMetaId = 1, TagId = 15},
        new ArticleTag() { Id = 5, ArticleMetaId = 1, TagId = 16},
        new ArticleTag() { Id = 6, ArticleMetaId = 2, TagId = 2},
        new ArticleTag() { Id = 7, ArticleMetaId = 3, TagId = 6},
        new ArticleTag() { Id = 8, ArticleMetaId = 2, TagId = 12},
        new ArticleTag() { Id = 9, ArticleMetaId = 2, TagId = 16},
        new ArticleTag() { Id = 10, ArticleMetaId = 3, TagId = 3},
        new ArticleTag() { Id = 11, ArticleMetaId = 4, TagId = 4},
        new ArticleTag() { Id = 12, ArticleMetaId = 5, TagId = 5},
        new ArticleTag() { Id = 13, ArticleMetaId = 5, TagId = 6},
        new ArticleTag() { Id = 14, ArticleMetaId = 7, TagId = 7},
        new ArticleTag() { Id = 15, ArticleMetaId = 8, TagId = 8},
        new ArticleTag() { Id = 16, ArticleMetaId = 9, TagId = 9},
        new ArticleTag() { Id = 17, ArticleMetaId = 10, TagId = 10},
    ];

    public static List<Category> Categories = [
        new Category() { Id = 1, Name = "Category 1", Description = "Description 1", ImageUrl = "", SortNumber = 1, IsHidden = 0},
        new Category() { Id = 2, Name = "Category 2", Description = "Description 2", ImageUrl = "", SortNumber = 2, IsHidden = 0},
        new Category() { Id = 3, Name = "Category 3", Description = "Description 3", ImageUrl = "", SortNumber = 3, IsHidden = 0},
    ];

    public static List<Topic> Topics = [
        new Topic() { Id = 1, Name = "Topic 1", Description = "Description 1", CategoryId = 1, ImageUrl = "", SortNumber = 1, IsHidden = false},
        new Topic() { Id = 2, Name = "Topic 2", Description = "Description 2", CategoryId = 1, ImageUrl = "", SortNumber = 2, IsHidden = false},
        new Topic() { Id = 3, Name = "Topic 3", Description = "Description 3", CategoryId = 1, ImageUrl = "", SortNumber = 3, IsHidden = false},
        new Topic() { Id = 4, Name = "Topic 4", Description = "Description 4", CategoryId = 2, ImageUrl = "", SortNumber = 4, IsHidden = false},
        new Topic() { Id = 5, Name = "Topic 5", Description = "Description 5", CategoryId = 2, ImageUrl = "", SortNumber = 5, IsHidden = false},
    ];

    public static List<ArticleMeta> ArticleMetas = [
        new ArticleMeta() { Id = 1, ArticleId = 1, TopicId = 1, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 1, IsHidden = false},
        new ArticleMeta() { Id = 2, ArticleId = 2, TopicId = 2, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 2, IsHidden = false},
        new ArticleMeta() { Id = 3, ArticleId = 3, TopicId = 3, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 3, IsHidden = false},
        new ArticleMeta() { Id = 4, ArticleId = 4, TopicId = 4, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 4, IsHidden = false},
        new ArticleMeta() { Id = 5, ArticleId = 5, TopicId = 5, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 5, IsHidden = false},
        new ArticleMeta() { Id = 6, ArticleId = 6, TopicId = 1, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 6, IsHidden = false},
        new ArticleMeta() { Id = 7, ArticleId = 7, TopicId = 2, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 7, IsHidden = false},
        new ArticleMeta() { Id = 8, ArticleId = 8, TopicId = 3, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 8, IsHidden = false},
        new ArticleMeta() { Id = 9, ArticleId = 9, TopicId = 4, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 9, IsHidden = false},
        new ArticleMeta() { Id = 10, ArticleId = 10, TopicId = 5, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 10, IsHidden = false},
        new ArticleMeta() { Id = 11, ArticleId = 11, TopicId = 1, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 11, IsHidden = false},
        new ArticleMeta() { Id = 12, ArticleId = 12, TopicId = 2, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 12, IsHidden = false},
        new ArticleMeta() { Id = 13, ArticleId = 13, TopicId = 3, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 13, IsHidden = false},
        new ArticleMeta() { Id = 14, ArticleId = 14, TopicId = 2, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 14, IsHidden = false},
        new ArticleMeta() { Id = 15, ArticleId = 15, TopicId = 4, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 15, IsHidden = false},
        new ArticleMeta() { Id = 16, ArticleId = 16, TopicId = 5, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 16, IsHidden = false},
        new ArticleMeta() { Id = 17, ArticleId = 17, TopicId = 1, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 17, IsHidden = false},
        new ArticleMeta() { Id = 18, ArticleId = 18, TopicId = 2, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 18, IsHidden = false},
        new ArticleMeta() { Id = 19, ArticleId = 19, TopicId = 3, IsTopicSummary = false, ImageUrl = "", UserId = 2, SortNumber = 19, IsHidden = false},
        new ArticleMeta() { Id = 20, ArticleId = 20, TopicId = 4, IsTopicSummary = false, ImageUrl = "", UserId = 1, SortNumber = 20, IsHidden = false},
    ];
}
