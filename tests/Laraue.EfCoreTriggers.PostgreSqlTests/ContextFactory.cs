using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    public class ContextFactory : BaseContextFactory<FinalContext>
    {
        public override FinalContext CreateDbContext() => new(new ContextOptionsFactory<DynamicDbContext>().CreateDbContextOptions());
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
                .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactoryDesignTimeSupport>()
                .Options;
        }
    }

    public class FinalContext : DynamicDbContext
    {
        public FinalContext(DbContextOptions<DynamicDbContext> options) 
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