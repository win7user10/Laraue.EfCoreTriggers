using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure
{
    public static class DynamicDbContextFactory
    {
        public static async Task<DynamicDbContext> GetDbContextAsync(IContextOptionsFactory<DynamicDbContext> optionsFactory, Action<ModelBuilder> setupDbContext)
        {
            var contextOptions = optionsFactory.CreateDbContextOptions();

            var context = new DynamicDbContext(contextOptions, setupDbContext);

            await context.Database.EnsureCreatedAsync();

            return context;
        }

    }
}