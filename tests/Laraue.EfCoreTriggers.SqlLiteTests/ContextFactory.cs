using Laraue.EfCoreTriggers.SqlLite.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    public class ContextFactory : BaseContextFactory<FinalContext>
    {
        public override FinalContext CreateDbContext() => new(new ContextOptionsFactory<DynamicDbContext>().CreateDbContextOptions());
    }

    public class FinalContext : DynamicDbContext
    {
        public FinalContext(DbContextOptions<DynamicDbContext> options)
            : base(options)
        {
        }
    }

    public class ContextOptionsFactory<TContext> : IContextOptionsFactory<TContext> where TContext : DbContext
    {
        public DbContextOptions<TContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseSqlite("Data Source=test.db;", x =>
                {
                    x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName);
                })
                .UseSnakeCaseNamingConvention()
                .UseSqlLiteTriggers()
                .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactoryDesignTimeSupport>()
                .Options;
        }
    }
}
