using Laraue.EfCoreTriggers.SqlLite.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    public class ContextFactory : BaseContextFactory<DynamicDbContext>
    {
        public override FinalContext CreateDbContext() => new();

        public class FinalContext : DynamicDbContext
        {
            public FinalContext()
                : base(new DbContextOptionsBuilder<DynamicDbContext>()
                    .UseSqlite("Filename=D://test.db", x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                    .UseSnakeCaseNamingConvention()
                    .UseSqlLiteTriggers()
                    .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactoryDesignTimeSupport>()
                    .Options)
            {
            }
        }
    }
}
