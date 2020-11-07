using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    public class ContextFactory : IDesignTimeDbContextFactory<NativeDbContext>
    {
        public NativeDbContext CreateDbContext(string[] args)
        {
            return CreatePgDbContext();
        }

        public NativeDbContext CreatePgDbContext()
        {
            var options = new DbContextOptionsBuilder<NativeDbContext>()
                .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=efcoretriggers;",
                    x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .UseTriggers()
                .Options;

            return new NativeDbContext(options);
        }
    }
}
