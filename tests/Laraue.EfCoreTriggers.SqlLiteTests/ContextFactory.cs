using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    public class ContextFactory : BaseContextFactory<NativeDbContext>
    {
        public override FinalContext CreateDbContext() => new();

        public class FinalContext : NativeDbContext
        {
            public FinalContext()
                : base(new DbContextOptionsBuilder<NativeDbContext>()
                    .UseSqlite("Filename=D://test.db", x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                    .UseSnakeCaseNamingConvention()
                    .UseTriggers()
                    .Options)
            {
            }
        }
    }
}
