using System;

namespace Server.Contents.Api.Infrastructure;

public static class ContentsContextTagExtensions
{
    public static async Task<List<Tag>> UpsertTagsAsync(this ContentsContext context, IEnumerable<string> names, TagType type = TagType.Default)
    {
        var nameList = names.Distinct().ToList();

        // 1. Fetch existing tags (Filtered by Name AND Type)
        var existingTags = await context.Tags
            .Where(t => nameList.Contains(t.Name) && t.Type == type)
            .ToListAsync();

        // 2. Identify truly new names
        var existingNames = existingTags.Select(t => t.Name).ToHashSet();
        var tagsToAdd = nameList
            .Where(name => !existingNames.Contains(name))
            .Select(name => new Tag { Name = name, Type = type })
            .ToList();

        if (!tagsToAdd.Any()) return existingTags;

        context.Tags.AddRange(tagsToAdd);

        try
        {
            await context.SaveChangesAsync();
            return existingTags.Concat(tagsToAdd).ToList();
        }
        catch (DbUpdateException)
        {
            // 3. Collision Handling: Something was inserted by another thread
            // Detach the failed entries to clean the tracker
            foreach (var entry in tagsToAdd)
            {
                context.Entry(entry).State = EntityState.Detached;
            }

            // 4. Re-fetch everything to ensure we return tags that actually exist in DB
            return await context.Tags
                .Where(t => nameList.Contains(t.Name) && t.Type == type)
                .ToListAsync();
        }
    }
}
