
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Server.Common.Extensions;
using Server.Common.Utils;
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
        await context.Categories.AddRangeAsync(SeedData.Categories);
        await context.Topics.AddRangeAsync(SeedData.Topics);
        await context.ArticleMetas.AddRangeAsync(SeedData.BuildArticleMetas(200));
        await context.ArticleMetas.AddRangeAsync(SeedData.BuildTopicSummary());

        await context.SaveChangesAsync();

    }
}

public class SeedData
{
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
    public static List<Category> Categories = [
        new Category() { Id = 1, Name = "Category 1", Description = "Description 1", ImageUrl = "", SortNumber = 1, IsHidden = false},
        new Category() { Id = 2, Name = "Category 2", Description = "Description 2", ImageUrl = "", SortNumber = 2, IsHidden = false},
        new Category() { Id = 3, Name = "Category 3", Description = "Description 3", ImageUrl = "", SortNumber = 3, IsHidden = false},
    ];

    public static List<Topic> Topics = [
        new Topic() { Id = 1, Name = "Topic 1", Description = "Description 1", CategoryId = 1, ImageUrl = "", SortNumber = 1, IsHidden = false},
        new Topic() { Id = 2, Name = "Topic 2", Description = "Description 2", CategoryId = 1, ImageUrl = "", SortNumber = 2, IsHidden = false},
        new Topic() { Id = 3, Name = "Topic 3", Description = "Description 3", CategoryId = 1, ImageUrl = "", SortNumber = 3, IsHidden = false},
        new Topic() { Id = 4, Name = "Topic 4", Description = "Description 4", CategoryId = 2, ImageUrl = "", SortNumber = 4, IsHidden = false},
        new Topic() { Id = 5, Name = "Topic 5", Description = "Description 5", CategoryId = 2, ImageUrl = "", SortNumber = 5, IsHidden = false},
        new Topic() { Id = 6, Name = "Topic 6", Description = "Description 6", CategoryId = 3, ImageUrl = "", SortNumber = 6, IsHidden = false},
        new Topic() { Id = 7, Name = "Topic 7", Description = "Description 7", CategoryId = 3, ImageUrl = "", SortNumber = 7, IsHidden = false},
        new Topic() { Id = 8, Name = "Topic 8", Description = "Description 8", CategoryId = 3, ImageUrl = "", SortNumber = 8, IsHidden = false},
    ];
    public static List<ArticleMeta> BuildArticleMetas(int articleCount)
    {
        var articleMetas = new List<ArticleMeta>();
        for (int i = 0; i < articleCount; i++)
        {
            var articleMetaId = SnowflakeId.GenerateId();
            var articleId = SnowflakeId.GenerateId();
            var articleMeta = new ArticleMeta()
            {
                Id = articleMetaId,
                ArticleId = articleId,
                TopicId = Topics[Random.Shared.Next(0, Topics.Count - 1)].Id,
                IsTopicSummary = false,
                ImageUrl = "",
                UserId = 1,
                SortNumber = i + 1,
                IsHidden = false,
                Article = new Article()
                {
                    Id = articleId,
                    Title = $"Article {i + 1}",
                    Description = $"Description {i + 1}",
                    Content = $"Content {i + 1}"
                },
                ArticleTags = new List<ArticleTag>()
                {
                    new ArticleTag(){ ArticleMetaId = articleMetaId, TagId = Tags[Random.Shared.Next(0, Tags.Count - 1)].Id },
                    new ArticleTag(){ ArticleMetaId = articleMetaId, TagId = Tags[Random.Shared.Next(0, Tags.Count - 1)].Id },
                }
            };
            articleMetas.Add(articleMeta);
        }

        return articleMetas;
    }
    public static List<ArticleMeta> BuildTopicSummary()
    {
        var articleMetas = new List<ArticleMeta>();
        for (int i = 0; i < Topics.Count; i++)
        {
            var articleMetaId = SnowflakeId.GenerateId();
            var articleId = SnowflakeId.GenerateId();
            var articleMeta = new ArticleMeta()
            {
                Id = articleMetaId,
                ArticleId = articleId,
                TopicId = Topics[i].Id,
                IsTopicSummary = true,
                ImageUrl = "",
                UserId = 1,
                SortNumber = 0,
                IsHidden = false,
                Article = new Article()
                {
                    Id = articleId,
                    Title = $"Topic {i + 1} Summary",
                    Description = $"Topic {i + 1} Summary Description",
                    Content = GetSampleContent(i + 1)
                }
            };
            articleMetas.Add(articleMeta);
        }

        return articleMetas;
    }
    private static string GetSampleContent(int id) =>
    $"#Article {id}#\n\n" +
    "## Basic Syntax\n\n" +
    "These are the elements outlined in John Gruber’s original design document. All Markdown applications support these elements.\n\n" +
    "### Heading\n\n" +
    "# H1\n" +
    "## H2\n" +
    "### H3\n\n" +
    "### Bold\n\n" +
    "**bold text**\n\n" +
    "### Italic\n\n" +
    "*italicized text*\n\n" +
    "### Blockquote\n\n" +
    "> blockquote\n\n" +
    "### Ordered List\n\n" +
    "1. First item\n" +
    "2. Second item\n" +
    "3. Third item\n\n" +
    "### Unordered List\n\n" +
    "- First item\n" +
    "- Second item\n" +
    "- Third item\n\n" +
    "### Code\n\n" +
    "`code`\n\n" +
    "### Horizontal Rule\n\n" +
    "---\n\n" +
    "### Link\n\n" +
    "[Markdown Guide](https://www.markdownguide.org)\n\n" +
    "### Image\n\n" +
    "![alt text](https://www.markdownguide.org/assets/images/tux.png)\n\n" +
    "## Extended Syntax\n\n" +
    "These elements extend the basic syntax by adding additional features. Not all Markdown applications support these elements.\n\n" +
    "### Table\n\n" +
    "| Syntax | Description |\n" +
    "| ----------- | ----------- |\n" +
    "| Header | Title |\n" +
    "| Paragraph | Text |\n\n" +
    "### Fenced Code Block\n\n" +
    "```\n" +
    "{\n" +
    "  \"firstName\": \"John\",\n" +
    "  \"lastName\": \"Smith\",\n" +
    "  \"age\": 25\n" +
    "}\n" +
    "```\n\n" +
    "### Footnote\n\n" +
    "Here's a sentence with a footnote. [^1]\n\n" +
    "[^1]: This is the footnote.\n\n" +
    "### Heading ID\n\n" +
    "### My Great Heading {#custom-id}\n\n" +
    "### Definition List\n\n" +
    "term\n" +
    ": definition\n\n" +
    "### Strikethrough\n\n" +
    "~~The world is flat.~~\n\n" +
    "### Task List\n\n" +
    "- [x] Write the press release\n" +
    "- [ ] Update the website\n" +
    "- [ ] Contact the media\n\n" +
    "### Emoji\n\n" +
    "That is so funny! :joy:\n\n" +
    "### Highlight\n\n" +
    "I need to highlight these ==very important words==.\n\n" +
    "### Subscript\n\n" +
    "H~2~O\n\n" +
    "### Superscript\n\n" +
    "X^2^\n";


}
