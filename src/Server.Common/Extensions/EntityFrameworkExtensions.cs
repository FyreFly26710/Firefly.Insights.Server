using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Server.Common.Types;

namespace Server.Common.Extensions;

public static class EntityFrameworkExtensions
{
    public static void ApplySoftDeleteQueryFilter(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
    public static IQueryable<TEntity> ApplyPageQuery<TEntity>(this IQueryable<TEntity> query, PageRequest pageInfo)
    {
        // Apply sorting if a SortField is provided
        if (!string.IsNullOrWhiteSpace(pageInfo.SortField))
        {
            var propertyInfo = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.Name.Equals(pageInfo.SortField, StringComparison.OrdinalIgnoreCase));

            if (propertyInfo != null)
            {
                query = pageInfo.SortOrder == SortOrder.asc
                    ? query.OrderBy(e => EF.Property<object>(e, propertyInfo.Name))
                    : query.OrderByDescending(e => EF.Property<object>(e, propertyInfo.Name));
            }
        }

        // Apply pagination
        return query.Skip((pageInfo.PageNumber - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
    }
    public static async Task<Paged<TEntity>> ToPagedAsync<TEntity>(this IQueryable<TEntity> query, PageRequest pageInfo) where TEntity : class
    {
        var totalCount = await query.CountAsync();
        var data = await query.ApplyPageQuery(pageInfo).ToListAsync();
        return new Paged<TEntity>()
        {
            PageRequest = pageInfo,
            TotalCount = totalCount,
            Data = data
        };
    }
}
