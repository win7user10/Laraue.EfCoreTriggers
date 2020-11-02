using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common
{
    class Initializer
    {
        public static DbProvider? UsingProvider { get; private set; }

        public static readonly Dictionary<string, DbProvider> KnownProviders = new Dictionary<string, DbProvider>
        {
            ["NpgsqlOptionsExtension"] = DbProvider.PostgreSql,
        };

        public static void SetProvider(DbProvider dbProvider)
        {
            UsingProvider = dbProvider;
        }

        public static void SetProvider(string providerName)
        {
            if (!KnownProviders.TryGetValue(providerName, out var dbProvider))
                throw new InvalidOperationException($"Extension {providerName} is not supporting!");

            SetProvider(dbProvider);
        }

        public static ITriggerSqlVisitor GetSqlProvider(IModel model)
        {
            return UsingProvider switch
            {
                DbProvider.PostgreSql => new PostgreSqlVisitor(model),
                null => throw new InvalidOperationException("DB provider hasn't been configured"),
                _ => throw new InvalidOperationException($"DB provider {UsingProvider} is not suppoting"),
            };
        }
    }
}
