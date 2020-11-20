using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    public class ContextFactory : BaseContextFactory<NativeDbContext>
    {
        public override NativeDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<NativeDbContext>()
                .UseSqlite("Filename=D://test.db", x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .UseTriggers()
                .Options;

            return new NativeDbContext(options);
        }
    }
}
