using Laraue.EfCoreTriggers.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.PostgreSql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UsePostgreSqlTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<PostgreSqlProvider, TContext>();
        }

        public static DbContextOptionsBuilder UsePostgreSqlTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseTriggers<PostgreSqlProvider>();
        }
    }
}