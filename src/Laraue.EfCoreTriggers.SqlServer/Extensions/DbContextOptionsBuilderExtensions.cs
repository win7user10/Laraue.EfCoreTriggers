using System;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.SqlServer.Migrations;
using Laraue.Linq2Triggers.Providers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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

            services.DecorateMigrationsSqlGeneratorForTriggers();
        }

        /// <summary>
        /// Wraps whichever <see cref="IMigrationsSqlGenerator"/> is currently registered with
        /// <see cref="TriggerSqlMigrationsSqlGenerator"/>, instead of replacing it outright.
        /// At the point this runs, that is normally the default <c>SqlServerMigrationsSqlGenerator</c>,
        /// so it gets wrapped and trigger DDL is fixed up automatically.
        /// If a consumer installs their own <see cref="IMigrationsSqlGenerator"/> - via
        /// <c>DbContextOptionsBuilder.ReplaceService</c> or via the <c>modifyServices</c> callback -
        /// EF Core applies that registration after this one runs, so it wins outright and is used
        /// unmodified (no trigger-DDL wrapping, but also no conflict with this library).
        /// </summary>
        /// <param name="services"></param>
        private static IServiceCollection DecorateMigrationsSqlGeneratorForTriggers(this IServiceCollection services)
        {
            var descriptor = services.LastOrDefault(x => x.ServiceType == typeof(IMigrationsSqlGenerator));

            if (descriptor is null)
            {
                return services;
            }

            services.Remove(descriptor);

            services.Add(new ServiceDescriptor(
                typeof(IMigrationsSqlGenerator),
                provider => new TriggerSqlMigrationsSqlGenerator(CreateInnerMigrationsSqlGenerator(descriptor, provider)),
                descriptor.Lifetime));

            return services;
        }

        private static IMigrationsSqlGenerator CreateInnerMigrationsSqlGenerator(ServiceDescriptor descriptor, IServiceProvider provider)
        {
            if (descriptor.ImplementationInstance is IMigrationsSqlGenerator instance)
            {
                return instance;
            }

            if (descriptor.ImplementationFactory is not null)
            {
                return (IMigrationsSqlGenerator)descriptor.ImplementationFactory(provider);
            }

            return (IMigrationsSqlGenerator)ActivatorUtilities.CreateInstance(provider, descriptor.ImplementationType!);
        }
    }
}
