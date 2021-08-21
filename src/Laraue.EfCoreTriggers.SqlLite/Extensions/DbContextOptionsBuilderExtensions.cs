using System;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlLite.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseSqlLiteTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, Action<AvailableConverters> setupConverters = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<SqlLiteProvider, TContext>(setupConverters);
        }

        public static DbContextOptionsBuilder UseSqlLiteTriggers(this DbContextOptionsBuilder optionsBuilder, Action<AvailableConverters> setupConverters = null)
        {
            return optionsBuilder.UseTriggers<SqlLiteProvider>(setupConverters);
        }
    }
}