using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common
{
    public class TriggersInitializer
    {
        private static readonly Dictionary<string, DbProvider> ProvidersAnnotations = new Dictionary<string, DbProvider>
        {
            ["Npgsql:ValueGenerationStrategy"] = DbProvider.PostgreSql,
            ["SqlServer:ValueGenerationStrategy"] = DbProvider.SqlServer,
        };

        public static ITriggerSqlVisitor GetSqlProvider(IModel model)
        {
            DbProvider? provider = null;
            foreach (var providerAnnotation in ProvidersAnnotations)
            {
                var annotation = model.FindAnnotation(providerAnnotation.Key);
                if (annotation != null)
                {
                    provider = providerAnnotation.Value;
                    break;
                }
            }

            if (provider is null)
                throw new InvalidOperationException($"Not found one of annotation {string.Join(", ", ProvidersAnnotations.Keys)} to recognize DB provider.");

            return provider switch
            {
                DbProvider.PostgreSql => new PostgreSqlVisitor(model),
                DbProvider.SqlServer => new SqlServerSqlVisitor(model),
                _ => throw new NotSupportedException($"Provider {provider} is not supported!"),
            };
        }
    }
}
