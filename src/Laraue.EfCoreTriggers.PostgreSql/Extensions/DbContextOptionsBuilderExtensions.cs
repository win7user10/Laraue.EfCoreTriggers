using System;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.PostgreSql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UsePostgreSqlTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, Action<AvailableConverters> setupConverters = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<PostgreSqlProvider, TContext>(setupConverters);
        }

        public static DbContextOptionsBuilder UsePostgreSqlTriggers(this DbContextOptionsBuilder optionsBuilder, Action<AvailableConverters> setupConverters = null)
        {
            return optionsBuilder.UseTriggers<PostgreSqlProvider>(setupConverters);
        }
    }
}