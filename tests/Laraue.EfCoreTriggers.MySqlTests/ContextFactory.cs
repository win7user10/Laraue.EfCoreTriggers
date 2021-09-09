using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    public class ContextFactory : BaseContextFactory<DynamicDbContext>
    {
        public override FinalContext CreateDbContext() => new();

        public class FinalContext : DynamicDbContext
        {
            public FinalContext()
                : base(new DbContextOptionsBuilder<DynamicDbContext>()
                    .UseMySql("server=localhost;user=mysql;password=mysql;database=efcoretriggers;", new MySqlServerVersion(new Version(8, 0, 22)),
                        x => x
                            .MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                    .UseSnakeCaseNamingConvention()
                    .UseMySqlTriggers()
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactoryDesignTimeSupport>()
                    .Options)
            {
            }
        }
    }
}
