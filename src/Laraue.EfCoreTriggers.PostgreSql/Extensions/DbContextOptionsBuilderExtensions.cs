using System;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.Triggers.PostgreSql.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.PostgreSql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers PostgreSQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UsePostgreSqlTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseEfCoreTriggers(AddPostgreSqlServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers PostgreSQL provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UsePostgreSqlTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
        {
            return optionsBuilder.UseEfCoreTriggers(AddPostgreSqlServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers PostgreSQL provider services.
        /// </summary>
        /// <param name="services"></param>
        public static void AddPostgreSqlServices(this IServiceCollection services)
        {
            services
                .AddEfCoreTriggerAdapters()
                .AddBasePostgreSqlServices();
        }
    }
}