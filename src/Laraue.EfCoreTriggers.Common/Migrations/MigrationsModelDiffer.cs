using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Laraue.EfCoreTriggers.Common.Migrations
{
    public class MigrationsModelDiffer : Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer
    {
        public MigrationsModelDiffer(
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotations,
            IChangeDetector changeDetector,
            IUpdateAdapterFactory updateAdapterFactory,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies)
                : base (typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory, commandBatchPreparerDependencies)
        {
        }

        public override IReadOnlyList<MigrationOperation> GetDifferences(IRelationalModel source, IRelationalModel target)
        {
            var deleteTriggerOperations = new List<SqlOperation>();
            var createTriggerOperations = new List<SqlOperation>();

            var sourceModel = source?.Model;
            var targetModel = target?.Model;

            var oldEntityTypeNames = sourceModel?.GetEntityTypes().Select(x => x.Name) ?? Enumerable.Empty<string>();
            var newEntityTypeNames = targetModel?.GetEntityTypes().Select(x => x.Name) ?? Enumerable.Empty<string>();

            var commonEntityTypeNames = oldEntityTypeNames.Intersect(newEntityTypeNames);

            // Drop all triggers for deleted entities.
            foreach (var deletedTypeName in oldEntityTypeNames.Except(commonEntityTypeNames))
            {
                var deletedEntityType = source.Model.FindEntityType(deletedTypeName);
                foreach (var annotation in deletedEntityType.GetTriggerAnnotations())
                    deleteTriggerOperations.AddDeleteTriggerSqlMigration(annotation, sourceModel);
            }

            // Add all triggers to created entities.
            foreach (var newTypeName in newEntityTypeNames.Except(commonEntityTypeNames))
            {
                var entityType = targetModel.FindEntityType(newTypeName);
                foreach (var annotation in targetModel.FindEntityType(newTypeName).GetTriggerAnnotations())
                    createTriggerOperations.AddCreateTriggerSqlMigration(annotation, entityType);
            }

            // For existing entities.
            foreach (var entityTypeName in commonEntityTypeNames)
            {
                var oldEntityType = sourceModel.FindEntityType(entityTypeName);
                var newEntityType = targetModel.FindEntityType(entityTypeName);

                var oldAnnotationNames = sourceModel.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name);

                var newAnnotationNames = targetModel.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name);

                var commonAnnotationNames = oldAnnotationNames.Intersect(newAnnotationNames);

                // If trigger was changed, recreate it.
                foreach (var commonAnnotationName in commonAnnotationNames)
                {
                    var oldValue = sourceModel.FindEntityType(entityTypeName).GetAnnotation(commonAnnotationName);
                    var newValue = targetModel.FindEntityType(entityTypeName).GetAnnotation(commonAnnotationName);

                    var oldTrigger = oldValue.ConvertTriggerToSql(oldEntityType);
                    var newTrigger = newValue.ConvertTriggerToSql(newEntityType);
                    if (oldTrigger != newTrigger)
                    {
                        deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldValue, sourceModel);
                        createTriggerOperations.AddCreateTriggerSqlMigration(newValue, newEntityType);
                    }
                }

                // If trigger was removed, delete it.
                foreach (var oldTriggerName in oldAnnotationNames.Except(commonAnnotationNames))
                {
                    var oldTriggerAnnotation = oldEntityType.GetAnnotation(oldTriggerName);
                    deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldTriggerAnnotation, sourceModel);
                }

                // If trigger was added, create it.
                foreach (var newTriggerName in newAnnotationNames.Except(commonAnnotationNames))
                {
                    var newTriggerAnnotation = newEntityType.GetAnnotation(newTriggerName);
                    createTriggerOperations.AddCreateTriggerSqlMigration(newTriggerAnnotation, newEntityType);
                }
            }

            return MergeOperations(base.GetDifferences(source, target), createTriggerOperations, deleteTriggerOperations);
        }

        private IReadOnlyList<MigrationOperation> MergeOperations(
            IEnumerable<MigrationOperation> migrationOperations,
            IEnumerable<MigrationOperation> createTriggersOperations,
            IEnumerable<MigrationOperation> deleteTriggersOperation)
        {
            return new List<MigrationOperation>(deleteTriggersOperation)
                .Concat(migrationOperations)
                .Concat(createTriggersOperations)
                .ToList();
        }
    }

    public static class MigrationExtensions
    {
        public static string ConvertTriggerToSql(this IAnnotation annotation, IEntityType entityType)
        {
            if (annotation.Value is ISqlConvertible configuredTrigger){
                return configuredTrigger.BuildSql(TriggerExtensions.GetSqlProvider(entityType.Model)).Sql;
            }

            throw new InvalidOperationException("The configured trigger cannot be converted to SQL");
        }
    }

    public static class IListExtensions
    {
        public static IEnumerable<IAnnotation> GetTriggerAnnotations(this IEntityType entityType)
        {
            return entityType.GetAnnotations()
                .Where(x => x.Name.StartsWith(Constants.AnnotationKey));
        }

        public static IList<SqlOperation> AddCreateTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation, IEntityType entityType)
        {
            var triggerSql = annotation.ConvertTriggerToSql(entityType);

            list.Add(new SqlOperation 
            {
                Sql = triggerSql,
            });
            return list;
        }

        public static IList<SqlOperation> AddDeleteTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation, IReadOnlyModel model)
        {
            list.Add(new SqlOperation
            {
                Sql = TriggerExtensions.GetSqlProvider(model).GetDropTriggerSql(annotation.Name),
            });
            return list;
        }
    }
}