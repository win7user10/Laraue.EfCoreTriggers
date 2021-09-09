using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    public class ContextFactory : BaseContextFactory<DynamicDbContext>
    {
        public override FinalContext CreateDbContext() => new();

        public class FinalContext : DynamicDbContext
        {
            public FinalContext()
                : base(new DbContextOptionsBuilder<DynamicDbContext>()
                    .UseSqlServer("Data Source=(LocalDb)\\v15.0;Database=EfCoreTriggers;Integrated Security=SSPI;",
                        x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                    .UseSnakeCaseNamingConvention()
                    .UseSqlServerTriggers()
                    .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactoryDesignTimeSupport>()
                    .Options)
            {
            }
        }
    }
}