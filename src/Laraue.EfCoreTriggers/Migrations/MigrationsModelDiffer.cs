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

namespace Laraue.EfCoreTriggers.Migrations
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

        private readonly Type[] _operationsOrder =
        {
            typeof(DropTableOperation),
            typeof(DropIndexOperation),
            typeof(DropPrimaryKeyOperation),
            typeof(DropSequenceOperation),
            typeof(DropUniqueConstraintOperation),
            typeof(DropCheckConstraintOperation),
            typeof(DropForeignKeyOperation),
            typeof(DeleteDataOperation),
            typeof(DropColumnOperation),
            typeof(EnsureSchemaOperation),
            typeof(RenameTableOperation),
            typeof(RenameColumnOperation),
            typeof(RenameIndexOperation),
            typeof(RenameSequenceOperation),
            typeof(AlterDatabaseOperation),
            typeof(CreateSequenceOperation),
            typeof(AlterTableOperation),
            typeof(ColumnOperation),
            typeof(AddPrimaryKeyOperation),
            typeof(AddUniqueConstraintOperation),
            typeof(AlterSequenceOperation),
            typeof(RestartSequenceOperation),
            typeof(CreateTableOperation),
            typeof(InsertDataOperation),
            typeof(UpdateDataOperation),
            typeof(AddForeignKeyOperation),
            typeof(CreateIndexOperation),
        };

        public override IReadOnlyList<MigrationOperation> GetDifferences(IModel source, IModel target)
        {
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
                    .SafeForEach(annotation => deleteTriggerOperations.AddDeleteTriggerSqlMigration(annotation, deletedEntityType.ClrType, source));
            }

            // Add all triggers to created entities.
            foreach (var newTypeName in newEntityTypeNames.Except(commonEntityTypeNames))
            {
                target.FindEntityType(newTypeName).GetTriggerAnnotations()
                    .SafeForEach(annotation => createTriggerOperations.AddCreateTriggerSqlMigration(annotation));
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
                        deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldValue, clrType, source);
                        createTriggerOperations.AddCreateTriggerSqlMigration(newValue);
                    }
                }

                // If trigger was removed, delete it.
                foreach (var oldTriggerName in oldAnnotationNames.Except(commonAnnotationNames))
                {
                    var oldTriggerAnnotation = oldEntityType.GetAnnotation(oldTriggerName);
                    deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldTriggerAnnotation, clrType, source);
                }

                // If trigger was added, create it.
                foreach (var newTriggerName in newAnnotationNames.Except(commonAnnotationNames))
                {
                    var newTriggerAnnotation = newEntityType.GetAnnotation(newTriggerName);
                    createTriggerOperations.AddCreateTriggerSqlMigration(newTriggerAnnotation);
                }
            }

            return MergeOperations(base.GetDifferences(source, target), createTriggerOperations, deleteTriggerOperations);
        }

        private IReadOnlyList<MigrationOperation> MergeOperations(
            IReadOnlyList<MigrationOperation> migrationOperations,
            IReadOnlyList<MigrationOperation> createTriggersOperations,
            IReadOnlyList<MigrationOperation> deleteTriggersOperation)
        {
            var operationsOrder = _operationsOrder.Select((value, index) => new { value, index })
                .ToDictionary(x => x.value, x => x.index);

            operationsOrder.TryGetValue(typeof(DeleteDataOperation), out int deleteOperationOrder);
            operationsOrder.TryGetValue(typeof(InsertDataOperation), out int insertOperationOrder);

            var operations = new List<MigrationOperation>();

            // First, should be executed all operations including delete data, because when 
            // data is dropping thiggers should be fired.
            foreach (var migrationOperation in migrationOperations)
            {
                if (operationsOrder.TryGetValue(migrationOperation.GetType(), out int order) && order > deleteOperationOrder)
                    break;
                operations.Add(migrationOperation);
            }

            operations.AddRange(deleteTriggersOperation);

            foreach (var migrationOperation in migrationOperations.Except(operations))
            {
                if (operationsOrder.TryGetValue(migrationOperation.GetType(), out int order) && order >= insertOperationOrder)
                    break;
                operations.Add(migrationOperation);
            }

            operations.AddRange(createTriggersOperations);
            operations.AddRange(migrationOperations.Except(operations));

            return operations;
        }
    }

    public static class IListExtensions
    {
        public static IEnumerable<IAnnotation> GetTriggerAnnotations(this IEntityType entityType)
        {
            return entityType.GetAnnotations()
                .Where(x => x.Name.StartsWith(Constants.AnnotationKey));
        }

        public static IList<SqlOperation> AddCreateTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation)
        {
            list.Add(new SqlOperation 
            {
                Sql = annotation.Value.ToString(),
            });
            return list;
        }

        public static IList<SqlOperation> AddDeleteTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation, Type entityType, IModel model)
        {
            list.Add(new SqlOperation
            {
                Sql = TriggersInitializer.GetSqlProvider(model).GetDropTriggerSql(annotation.Name, entityType),
            });
            return list;
        }
    }
}