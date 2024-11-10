using App.Application.Contracts.Persistence;
using App.Persistence;

namespace App.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}

