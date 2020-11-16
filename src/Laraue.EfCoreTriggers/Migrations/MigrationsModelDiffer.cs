using Laraue.EfCoreTriggers.Common;
using Laraue.Core.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Builders.Providers;
using Laraue.EfCoreTriggers.Common.Builders.Triggers.Base;

namespace Laraue.EfCoreTriggers.Migrations
{
    public class MigrationsModelDiffer : Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer
    {
        private readonly ITriggerProvider _triggerProvider;

        public MigrationsModelDiffer(
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotations,
            IChangeDetector changeDetector,
            IUpdateAdapterFactory updateAdapterFactory,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies,
            ITriggerProvider triggerProvider)
                : base (typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory, commandBatchPreparerDependencies)
        {
            _triggerProvider = triggerProvider ?? throw new ArgumentNullException(nameof(triggerProvider));
        }

        public override IReadOnlyList<MigrationOperation> GetDifferences(IModel source, IModel target)
        {
            _triggerProvider.SetModel(target);

            var deleteTriggerOperations = new List<SqlOperation>();
            var createTriggerOperations = new List<SqlOperation>();

            var oldEntityTypeNames = source?.GetEntityTypes().Select(x => x.Name) ?? Enumerable.Empty<string>();
            var newEntityTypeNames = target?.GetEntityTypes().Select(x => x.Name) ?? Enumerable.Empty<string>();

            var commonEntityTypeNames = oldEntityTypeNames.Intersect(newEntityTypeNames);

            // Drop all triggers for deleted entities.
            foreach (var deletedTypeName in oldEntityTypeNames.Except(commonEntityTypeNames))
            {
                var deletedEntityType = source.FindEntityType(deletedTypeName);
                deletedEntityType.GetTriggerAnnotations()
                    .SafeForEach(annotation => deleteTriggerOperations.AddDeleteTriggerSqlMigration(annotation, deletedEntityType.ClrType, _triggerProvider));
            }

            // Add all triggers to created entities.
            foreach (var newTypeName in newEntityTypeNames.Except(commonEntityTypeNames))
            {
                target.FindEntityType(newTypeName).GetTriggerAnnotations()
                    .SafeForEach(annotation => createTriggerOperations.AddCreateTriggerSqlMigration(annotation, _triggerProvider));
            }

            // For existing entities.
            foreach (var entityTypeName in commonEntityTypeNames)
            {
                var clrType = target.FindEntityType(entityTypeName).ClrType
                    ?? source.FindEntityType(entityTypeName).ClrType
                    ?? throw new InvalidOperationException($"Unknow Clr type for entity {entityTypeName}");

                var oldEntityType = source.FindEntityType(entityTypeName);
                var newEntityType = target.FindEntityType(entityTypeName);

                var oldAnnotationNames = source.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name);

                var newAnnotationNames = target.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name);

                var commonAnnotationNames = oldAnnotationNames.Intersect(newAnnotationNames);

                // If trigger was changed, recreate it.
                foreach (var commonAnnotationName in commonAnnotationNames)
                {
                    var oldValue = source.FindEntityType(entityTypeName).GetAnnotation(commonAnnotationName);
                    var newValue = target.FindEntityType(entityTypeName).GetAnnotation(commonAnnotationName);
                    if ((string)oldValue.Value != (string)newValue.Value)
                    {
                        deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldValue, clrType, _triggerProvider);
                        createTriggerOperations.AddCreateTriggerSqlMigration(newValue, _triggerProvider);
                    }
                }

                // If trigger was removed, delete it.
                foreach (var oldTriggerName in oldAnnotationNames.Except(commonAnnotationNames))
                {
                    var oldTriggerAnnotation = oldEntityType.GetAnnotation(oldTriggerName);
                    deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldTriggerAnnotation, clrType, _triggerProvider);
                }

                // If trigger was added, create it.
                foreach (var newTriggerName in newAnnotationNames.Except(commonAnnotationNames))
                {
                    var newTriggerAnnotation = newEntityType.GetAnnotation(newTriggerName);
                    createTriggerOperations.AddCreateTriggerSqlMigration(newTriggerAnnotation, _triggerProvider);
                }
            }

            return MergeOperations(base.GetDifferences(source, target), createTriggerOperations, deleteTriggerOperations);
        }

        private IReadOnlyList<MigrationOperation> MergeOperations(
            IReadOnlyList<MigrationOperation> migrationOperations,
            IReadOnlyList<MigrationOperation> createTriggersOperations,
            IReadOnlyList<MigrationOperation> deleteTriggersOperation)
        {
            return new List<MigrationOperation>(deleteTriggersOperation)
                .Concat(migrationOperations)
                .Concat(createTriggersOperations)
                .ToList();
        }
    }

    public static class IListExtensions
    {
        public static IEnumerable<IAnnotation> GetTriggerAnnotations(this IEntityType entityType)
        {
            return entityType.GetAnnotations()
                .Where(x => x.Name.StartsWith(Constants.AnnotationKey));
        }

        public static IList<SqlOperation> AddCreateTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation, ITriggerProvider triggerProvider)
        {
            var trigger = annotation.Value as ISqlConvertible;

            list.Add(new SqlOperation 
            {
                Sql = trigger.BuildSql(triggerProvider),
            });
            return list;
        }

        public static IList<SqlOperation> AddDeleteTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation, Type entityType, ITriggerProvider triggerProvider)
        {
            list.Add(new SqlOperation
            {
                Sql = triggerProvider.GetDropTriggerSql(annotation.Name, entityType),
            });
            return list;
        }
    }
}