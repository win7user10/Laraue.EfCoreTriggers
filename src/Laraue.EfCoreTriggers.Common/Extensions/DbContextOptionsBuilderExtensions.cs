using System;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Migrations;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseTriggers<TTriggerProvider, TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, Action<AvailableConverters> setupConverters)
            where TTriggerProvider : ITriggerProvider
            where TContext : DbContext
        {
            UseTriggers<TTriggerProvider>(optionsBuilder, setupConverters);
            return optionsBuilder;
        }

        public static DbContextOptionsBuilder UseTriggers<TTriggerProvider>(this DbContextOptionsBuilder optionsBuilder, Action<AvailableConverters> setupConverters)
            where TTriggerProvider : ITriggerProvider
        {
            TriggerExtensions.RememberTriggerProvider<TTriggerProvider>(setupConverters);
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
    }
}
