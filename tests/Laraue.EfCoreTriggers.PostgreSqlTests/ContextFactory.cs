using Laraue.EfCoreTriggers.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    public class ContextFactory : BaseContextFactory<NativeDbContext>
    {
        public override FinalContext CreateDbContext() => new();
    }

    public class FinalContext : NativeDbContext
    {
        public FinalContext() 
            : base(new DbContextOptionsBuilder<NativeDbContext>()
                .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=efcoretriggers;",
                    x => x.MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .UseTriggers()
                .Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            base.OnModelCreating(modelBuilder);
        }
    }
}