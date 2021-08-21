using System;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.MySql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseMySqlTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, Action<AvailableConverters> setupConverters = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<MySqlProvider, TContext>(setupConverters);
        }

        public static DbContextOptionsBuilder UseMySqlTriggers(this DbContextOptionsBuilder optionsBuilder, Action<AvailableConverters> setupConverters = null)
        {
            return optionsBuilder.UseTriggers<MySqlProvider>(setupConverters);
        }
    }
}