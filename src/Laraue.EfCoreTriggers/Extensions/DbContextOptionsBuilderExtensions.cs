using Laraue.EfCoreTriggers.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Laraue.EfCoreTriggers.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            UseTriggers((DbContextOptionsBuilder)optionsBuilder);
            return optionsBuilder;
        }

        public static DbContextOptionsBuilder UseTriggers(this DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.ReplaceMigrationsService();

        private static DbContextOptionsBuilder ReplaceMigrationsService(this DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
    }
}
