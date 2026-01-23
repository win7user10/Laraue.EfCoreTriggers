using Laraue.EfCoreTriggers.Oracle.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.OracleTests
{
    public class ContextFactory : BaseContextFactory<DynamicDbContext>
    {
#if (NETSTANDARD)
        public override DynamicDbContext CreateDbContext() => new(new ContextOptionsFactory<DynamicDbContext>().CreateDbContextOptions());
#else
        public override FinalContext CreateDbContext() => new(new ContextOptionsFactory<DynamicDbContext>().CreateDbContextOptions());
#endif
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
                .UseOracle("Data Source=localhost;User Id=oracle;Password=oracle;Integrated Security=no;",
                    x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                .UseOracleTriggers()
                .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactoryDesignTimeSupport>()
                .Options;
        }
    }
}