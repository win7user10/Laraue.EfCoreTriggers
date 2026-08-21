using System;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerTriggerMigrationsSqlGeneratorTests
    {
        [Fact]
        public void CreateTriggerSql_IsWrappedInExec_SoItCanRunInsideIdempotentMigrationScripts()
        {
            var commandText = GenerateUpMigrationSql(x => x
                .AfterInsert(trigger => trigger
                    .Action(action => action
                        .Insert(inserted => new DestinationEntity { StringField = "12" }))));

            Assert.Contains("EXEC(N'CREATE TRIGGER", commandText, StringComparison.Ordinal);
            Assert.DoesNotContain("\nCREATE TRIGGER", commandText, StringComparison.Ordinal);
        }

        [Fact]
        public void DropTriggerSql_IsWrappedInExec_SoItCanRunInsideIdempotentMigrationScripts()
        {
            var commandText = GenerateDownMigrationSql(x => x
                .AfterInsert(trigger => trigger
                    .Action(action => action
                        .Insert(inserted => new DestinationEntity { StringField = "12" }))));

            Assert.Contains("EXEC(N'DROP TRIGGER", commandText, StringComparison.Ordinal);
        }

        private static string GenerateUpMigrationSql(Action<EntityTypeBuilder<SourceEntity>> setupTrigger)
        {
            // Not disposed on purpose: DynamicDbContext.Dispose() runs migration
            // clean-up against a real database, which this test does not need.
            var context = CreateContext(setupTrigger);

            var relationalModel = context.GetService<IDesignTimeModel>().Model.GetRelationalModel();
            var differ = context.GetService<IMigrationsModelDiffer>();
            var operations = differ.GetDifferences(null, relationalModel);

            var generator = context.GetService<IMigrationsSqlGenerator>();
            var commands = generator.Generate(operations);

            return string.Join("\n", commands.Select(x => x.CommandText));
        }

        private static string GenerateDownMigrationSql(Action<EntityTypeBuilder<SourceEntity>> setupTrigger)
        {
            // Not disposed on purpose: DynamicDbContext.Dispose() runs migration
            // clean-up against a real database, which this test does not need.
            var context = CreateContext(setupTrigger);

            var relationalModel = context.GetService<IDesignTimeModel>().Model.GetRelationalModel();
            var differ = context.GetService<IMigrationsModelDiffer>();
            var operations = differ.GetDifferences(relationalModel, null);

            var generator = context.GetService<IMigrationsSqlGenerator>();
            var commands = generator.Generate(operations);

            return string.Join("\n", commands.Select(x => x.CommandText));
        }

        private static DynamicDbContext CreateContext(Action<EntityTypeBuilder<SourceEntity>> setupTrigger)
        {
            var options = new ContextOptionsFactory<DynamicDbContext>().CreateDbContextOptions();

            return new DynamicDbContext(options, modelBuilder => setupTrigger(modelBuilder.Entity<SourceEntity>()));
        }
    }
}
