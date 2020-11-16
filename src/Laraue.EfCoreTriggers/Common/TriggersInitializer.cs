using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common
{
    public class TriggersInitializer
    {
        public static ITriggerProvider GetSqlProvider(IModel model, DbProvider dbProvider)
        {
            return dbProvider switch
            {
                DbProvider.PostgreSql => new PostgreSqlProvider(model),
                DbProvider.SqlServer => new SqlServerProvider(model),
                _ => throw new NotSupportedException($"Provider {dbProvider} is not supported!"),
            };
        }
    }
}
