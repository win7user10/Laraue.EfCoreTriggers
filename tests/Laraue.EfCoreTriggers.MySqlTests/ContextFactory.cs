using System;
using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.MySqlTests;

public class ContextFactory : BaseContextFactory<DynamicDbContext>
{
    public override FinalContext CreateDbContext() => new(new ContextOptionsFactory<DynamicDbContext>().CreateDbContextOptions());
}

public class FinalContext : DynamicDbContext
{
    public FinalContext(DbContextOptions<DynamicDbContext> options)
        : base(options)
    {
    }
}

public class ContextOptionsFactory<TContext> : IContextOptionsFactory<TContext> where TContext : DbContext
{
    public DbContextOptions<TContext> CreateDbContextOptions()
    {
        return new DbContextOptionsBuilder<TContext>()
            .UseMySql("server=localhost;user=root;password=mysql;database=efcoretriggers;", new MySqlServerVersion(new Version(8, 0, 22)),
                x => x
                    .MigrationsAssembly(typeof(ContextFactory).Assembly.FullName))
            .UseMySqlTriggers()
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactoryDesignTimeSupport>()
            .Options;
    }
}