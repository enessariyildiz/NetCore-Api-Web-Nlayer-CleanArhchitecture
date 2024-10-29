using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Repositories.Interceptors
{
    public class AuditDbContextInterceptor: SaveChangesInterceptor
    {
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {




            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
