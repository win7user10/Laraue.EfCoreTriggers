using Laraue.EfCoreTriggers.Common;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Laraue.EfCoreTriggers.Extensions
{
    public static class TriggerExtensions
    {
        private static DbProvider _activeProvider;

        private static readonly Dictionary<string, DbProvider> ProviderOptionTypes = new Dictionary<string, DbProvider>
        {
            ["NpgsqlOptionsExtension"] = DbProvider.PostgreSql,
            ["SqlServerOptionsExtension"] = DbProvider.SqlServer,
            ["SqliteOptionsExtension"] = DbProvider.SqlLite,
            ["MySqlOptionsExtension"] = DbProvider.MySql,
        };

        public static DbProvider GetActiveDbProvider(this DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
                throw new InvalidOperationException("To use triggers, DB provider should be added");

            var dbProviderOptions = builder.Options.Extensions;

            var options = dbProviderOptions.Where(x => x.Info.IsDatabaseProvider)
                .SingleOrDefault();

            var dbProviderOptionsType = options.GetType().Name;

            if (ProviderOptionTypes.TryGetValue(dbProviderOptionsType, out var provider))
                return provider;

            throw new NotSupportedException($"Provider with options {dbProviderOptionsType} is not supported.");
        }

        /// <summary>
        /// Bad solution, but have no idea yet, how to register current provider using DbContextOptionsBuilder.
        /// </summary>
        /// <param name="builder"></param>
        public static void RememberActiveDbProvider(this DbContextOptionsBuilder builder)
        {
            _activeProvider = builder.GetActiveDbProvider();
        }

        public static ITriggerProvider GetSqlProvider(IModel model)
        {
            return _activeProvider switch
            {
                DbProvider.PostgreSql => new PostgreSqlProvider(model),
                DbProvider.SqlServer => new SqlServerProvider(model),
                DbProvider.SqlLite => new SqlLiteProvider(model),
                DbProvider.MySql => new MySqlProvider(model),
                _ => throw new NotSupportedException($"Provider {_activeProvider} is not supported!"),
            };
        }
    }
}
