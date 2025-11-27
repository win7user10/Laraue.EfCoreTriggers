using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.Linq2Triggers.Providers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.SqlServer.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers SQL Server provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UseSqlServerTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseEfCoreTriggers(AddSqlServerServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers SQL Server provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseSqlServerTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
        {
            return optionsBuilder.UseEfCoreTriggers(AddSqlServerServices, modifyServices);
        }
        
        /// <summary>
        /// Add EF Core triggers SQL Server provider services.
        /// </summary>
        /// <param name="services"></param>
        public static void AddSqlServerServices(this IServiceCollection services)
        {
            services
                .AddEfCoreTriggerAdapters()
                .AddBaseSqlServerServices();
        }
    }
}