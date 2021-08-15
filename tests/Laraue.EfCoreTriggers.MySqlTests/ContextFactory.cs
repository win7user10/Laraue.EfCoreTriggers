using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Laraue.EfCoreTriggers.MySql.Extensions;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    public class ContextFactory : BaseContextFactory<NativeDbContext>
    {
        public override FinalContext CreateDbContext() => new();

        public class FinalContext : NativeDbContext
        {
            public FinalContext()
                : base(new DbContextOptionsBuilder<NativeDbContext>()
                    .UseMySql("server=localhost;user=mysql;password=mysql;database=efcoretriggers;", new MySqlServerVersion(new Version(8, 0, 22)),
                        x => x
                            .MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                    .UseSnakeCaseNamingConvention()
                    .UseTriggers()
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .Options)
            {
            }
        }
    }
}
