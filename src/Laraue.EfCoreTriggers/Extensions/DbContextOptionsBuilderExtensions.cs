using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Linq;

namespace Laraue.EfCoreTriggers.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceMigrationsService();

            var providers = optionsBuilder.Options
                .Extensions
                .Where(x => x.Info.IsDatabaseProvider)
                .ToArray();

            if (providers.Length == 0) throw new InvalidOperationException("No one DB provider was found!");
            if (providers.Length > 1) throw new InvalidOperationException(
                $"Found {providers.Length} DB providers, try to chose provider explicitly using another overload.");

            TriggersInitializer.SetProvider(providers.First().GetType().Name);

            return optionsBuilder;
        }

        public static DbContextOptionsBuilder<TContext> UseTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            UseTriggers((DbContextOptionsBuilder)optionsBuilder);
            return optionsBuilder;
        }

        public static DbContextOptionsBuilder UseTriggers(this DbContextOptionsBuilder optionsBuilder, DbProvider dbProvider)
        {
            optionsBuilder.ReplaceMigrationsService();
            TriggersInitializer.SetProvider(dbProvider);
            return optionsBuilder;
        }

        public static DbContextOptionsBuilder<TContext> UseTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, DbProvider dbProvider)
            where TContext : DbContext
        {
            UseTriggers((DbContextOptionsBuilder)optionsBuilder, dbProvider);
            return optionsBuilder;
        }

        private static DbContextOptionsBuilder ReplaceMigrationsService(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
    }
}
