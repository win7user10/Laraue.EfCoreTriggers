using System;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure
{
    public static class DynamicDbContextFactory
    {
        public static DynamicDbContext GetDbContext(IContextOptionsFactory<DynamicDbContext> optionsFactory, Action<ModelBuilder> setupDbContext)
        {
            var contextOptions = optionsFactory.CreateDbContextOptions();

            var context = new DynamicDbContext(contextOptions, setupDbContext);

            context.Database.EnsureCreated();

            return context;
        }
    }
}