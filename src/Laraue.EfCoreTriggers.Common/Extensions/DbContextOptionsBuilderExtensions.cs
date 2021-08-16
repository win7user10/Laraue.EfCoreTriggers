using Laraue.EfCoreTriggers.Common.Migrations;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseTriggers<TTriggerProvider, TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TTriggerProvider : ITriggerProvider
            where TContext : DbContext
        {
            UseTriggers<TTriggerProvider>(optionsBuilder);
            return optionsBuilder;
        }

        public static DbContextOptionsBuilder UseTriggers<TTriggerProvider>(this DbContextOptionsBuilder optionsBuilder)
            where TTriggerProvider : ITriggerProvider
        {
            TriggerExtensions.RememberTriggerProviderType<TTriggerProvider>();
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
    }
}
