using System;
using Laraue.EfCoreTriggers.Common.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseTriggers(
            this DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
        
        public static DbContextOptionsBuilder<TContext> UseTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            return optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDiffer>();
        }
    }
}
