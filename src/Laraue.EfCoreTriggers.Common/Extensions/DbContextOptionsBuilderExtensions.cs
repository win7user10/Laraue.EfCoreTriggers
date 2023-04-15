using System;
using Laraue.EfCoreTriggers.Common.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    /// <summary>
    /// Extensions to configure EF Core <see cref="IServiceProvider"/>
    /// for using EF Core Triggers.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add implementation of <see cref="IMigrationsModelDiffer"/> 
        /// which allow to use triggers.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="addDefaultServices"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseEfCoreTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection> addDefaultServices,
            Action<IServiceCollection>? modifyServices)
        {
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder)
                .AddOrUpdateExtension(new EfCoreTriggersExtension(addDefaultServices, modifyServices));
            
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
        
        /// <summary>
        /// Add implementation of <see cref="IMigrationsModelDiffer"/> 
        /// which allow to use triggers.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="addDefaultServices"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UseEfCoreTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection> addDefaultServices,
            Action<IServiceCollection>? modifyServices)
            where TContext : DbContext
        {
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder)
                .AddOrUpdateExtension(new EfCoreTriggersExtension(addDefaultServices, modifyServices));
            
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
    }
}
