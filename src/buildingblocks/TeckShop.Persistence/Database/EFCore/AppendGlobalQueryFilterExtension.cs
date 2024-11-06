using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace TeckShop.Persistence.Database.EFCore
{
    internal static class ModelBuilderExtensions
    {
        public static ModelBuilder AppendGlobalQueryFilter<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> filter)
        {
            // get a list of entities without a baseType that implement the interface TInterface
            var entities = modelBuilder.Model.GetEntityTypes()
                .Where(entity => entity.BaseType is null && entity.ClrType.GetInterface(typeof(TInterface).Name) is not null)
                .Select(entity => entity.ClrType);

            foreach (var entity in entities)
            {
                var parameterType = Expression.Parameter(modelBuilder.Entity(entity).Metadata.ClrType);
                var filterBody = ReplacingExpressionVisitor.Replace(filter.Parameters[0], parameterType, filter.Body);

                // get the existing query filter
                if (modelBuilder.Entity(entity).Metadata.GetQueryFilter() is { } existingFilter)
                {
                    var existingFilterBody = ReplacingExpressionVisitor.Replace(existingFilter.Parameters[0], parameterType, existingFilter.Body);

                    // combine the existing query filter with the new query filter
                    filterBody = Expression.AndAlso(existingFilterBody, filterBody);
                }

                // apply the new query filter
                modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(filterBody, parameterType));
            }

            return modelBuilder;
        }
    }
}
