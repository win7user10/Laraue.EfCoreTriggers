using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    public class ContextFactory : BaseContextFactory<NativeDbContext>
    {
        public override NativeDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<NativeDbContext>()
                .UseSqlServer("Data Source=(LocalDb)\\v15.0;Database=EfCoreTriggers;Integrated Security=SSPI;",
                    x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .UseTriggers()
                .Options;

            return new NativeDbContext(options);
        }
    }
}