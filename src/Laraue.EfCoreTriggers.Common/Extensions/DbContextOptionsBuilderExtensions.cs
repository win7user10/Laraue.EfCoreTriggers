using System;
using Laraue.EfCoreTriggers.Common.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

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
        /// <returns></returns>
        public static DbContextOptionsBuilder ReplaceMigrationsModelDiffer(
            this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
        
        /// <summary>
        /// Add implementation of <see cref="IMigrationsModelDiffer"/> 
        /// which allow to use triggers.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> ReplaceMigrationsModelDiffer<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
    }
}
