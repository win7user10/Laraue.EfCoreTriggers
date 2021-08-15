using Microsoft.EntityFrameworkCore;
using Laraue.EfCoreTriggers.Extensions;

namespace Laraue.EfCoreTriggers.SqlServer.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            return optionsBuilder.UseTriggers<SqlServerProvider, TContext>();
        }

        public static DbContextOptionsBuilder UseTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseTriggers<SqlServerProvider>();
        }
    }
}