using Laraue.EfCoreTriggers.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlLite.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseSqlLiteTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<SqlLiteProvider, TContext>();
        }

        public static DbContextOptionsBuilder UseSqlLiteTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseTriggers<SqlLiteProvider>();
        }
    }
}