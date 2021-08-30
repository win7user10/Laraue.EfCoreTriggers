using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    public class ContextFactory : BaseContextFactory<FinalContext>
    {
        public override FinalContext CreateDbContext() => new(new ContextOptionsFactory<NativeDbContext>().CreateDbContextOptions());
    }

    public class ContextOptionsFactory<TContext> : IContextOptionsFactory<TContext> where TContext : DbContext
    {
        public DbContextOptions<TContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<TContext>()
                .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=efcoretriggers;",
                    x => x.MigrationsAssembly(typeof(TContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .UsePostgreSqlTriggers()
                .Options;
        }
    }

    public class FinalContext : NativeDbContext
    {
        public FinalContext(DbContextOptions<NativeDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            base.OnModelCreating(modelBuilder);
        }
    }
}