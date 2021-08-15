using Microsoft.EntityFrameworkCore;
using Laraue.EfCoreTriggers.Extensions;

namespace Laraue.EfCoreTriggers.PostgreSql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<PostgreSqlProvider, TContext>();
        }

        public static DbContextOptionsBuilder UseTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseTriggers<PostgreSqlProvider>();
        }
    }
}