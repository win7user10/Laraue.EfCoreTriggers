using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.Linq2Triggers.Providers.Oracle.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Oracle.Extensions
{
    /// <summary>
    /// Extensions to add Oracle Triggers to the container.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers Oracle provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseOracleTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection>? modifyServices = null)
        {
            return optionsBuilder.UseEfCoreTriggers(AddOracleServices,  modifyServices);
        }
        
        /// <summary>
        /// Add EF Core triggers Oracle provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>A
        public static DbContextOptionsBuilder<TContext> UseOracleTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection>? modifyServices = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseEfCoreTriggers(AddOracleServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers Oracle provider services.
        /// </summary>
        public static void AddOracleServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddEfCoreTriggerAdapters()
                .AddBaseOracleServices();
        }
    }
}