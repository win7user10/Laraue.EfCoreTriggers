using System;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlServer.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseSqlServerTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, Action<AvailableConverters> setupConverters = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<SqlServerProvider, TContext>(setupConverters);
        }

        public static DbContextOptionsBuilder UseSqlServerTriggers(this DbContextOptionsBuilder optionsBuilder, Action<AvailableConverters> setupConverters = null)
        {
            return optionsBuilder.UseTriggers<SqlServerProvider>(setupConverters);
        }
    }
}