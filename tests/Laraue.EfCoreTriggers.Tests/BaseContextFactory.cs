using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Laraue.EfCoreTriggers.Tests
{
    public interface IContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        TContext CreateDbContext();
    }

    public abstract class BaseContextFactory<TContext> : IContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
            => CreateDbContext();

        public abstract TContext CreateDbContext();
    }
}
