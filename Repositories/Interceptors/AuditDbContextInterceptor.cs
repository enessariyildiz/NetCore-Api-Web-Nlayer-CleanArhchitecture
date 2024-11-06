using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.Tracing;

namespace App.Repositories.Interceptors
{
    public class AuditDbContextInterceptor : SaveChangesInterceptor
    {

        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaviors = new()
        {
            {EntityState.Added,AddBehavior},
            {EntityState.Added,ModifiedBehavior},
        };

        private static void AddBehavior(DbContext context, IAuditEntity auidtEntity)
        {
            auidtEntity.Created = DateTime.Now;
            context.Entry(auidtEntity).Property(x => x.Updated).IsModified = false;

        }

        private static void ModifiedBehavior(DbContext context, IAuditEntity auidtEntity)
        {
            auidtEntity.Created = DateTime.Now;
            context.Entry(auidtEntity).Property(x => x.Created).IsModified = false;

        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {

            foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
            {

                if (entityEntry.Entity is not IAuditEntity auidtEntity) continue;

                if (entityEntry.State is not (EntityState.Added or EntityState.Modified)) continue;

                Behaviors[entityEntry.State](eventData.Context, auidtEntity);

            }

            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}

