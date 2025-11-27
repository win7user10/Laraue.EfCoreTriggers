using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.Linq2Triggers.Providers.MySql.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.MySql.Extensions
{
    /// <summary>
    /// Extensions to add MySql Triggers to the container.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers MySQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseMySqlTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection>? modifyServices = null)
        {
            return optionsBuilder.UseEfCoreTriggers(AddMySqlServices,  modifyServices);
        }
        
        /// <summary>
        /// Add EF Core triggers MySQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>A
        public static DbContextOptionsBuilder<TContext> UseMySqlTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection>? modifyServices = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseEfCoreTriggers(AddMySqlServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers MySQL provider services.
        /// </summary>
        public static void AddMySqlServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddEfCoreTriggerAdapters()
                .AddBaseMySqlServices();
        }
    }
}