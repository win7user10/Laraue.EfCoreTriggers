using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    public class ContextFactory : IDesignTimeDbContextFactory<NativeDbContext>
    {
        public NativeDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public NativeDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<NativeDbContext>()
                .UseSqlServer("Data Source=(LocalDb)\\v15.0",
                    x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .UseTriggers()
                .Options;

            return new NativeDbContext(options);
        }
    }
}