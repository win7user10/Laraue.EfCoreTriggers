using Microsoft.EntityFrameworkCore;
using Laraue.EfCoreTriggers.Extensions;

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