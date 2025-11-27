using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.Linq2Triggers.Providers.Sqlite.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.SqlLite.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers SQLite provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UseSqlLiteTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseEfCoreTriggers(AddSqliteServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers SQLite provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseSqlLiteTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
        {
            return optionsBuilder.UseEfCoreTriggers(AddSqliteServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers SQLite provider services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void AddSqliteServices(this IServiceCollection services)
        {
            services
                .AddEfCoreTriggerAdapters()
                .AddBaseSqliteServices();
        }
    }
}