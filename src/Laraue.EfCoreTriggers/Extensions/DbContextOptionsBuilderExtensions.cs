using Laraue.EfCoreTriggers.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

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
            => optionsBuilder.UseTriggersInternal();

        private static DbContextOptionsBuilder UseTriggersInternal(this DbContextOptionsBuilder optionsBuilder)
        {
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(new TriggersExtension(optionsBuilder));
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
                //.ReplaceService<IDesignTimeServices, MyDesignTimeServices>();
        }

        
    }
}
