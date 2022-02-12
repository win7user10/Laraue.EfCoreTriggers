using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Extensions;
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
    /// <inheritdoc />
    public class MigrationsModelDiffer : Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer
    {
        /// <inheritdoc />
        public MigrationsModelDiffer(
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotations,
            IChangeDetector changeDetector,
            IUpdateAdapterFactory updateAdapterFactory,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies)
                : base (typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory, commandBatchPreparerDependencies)
        {
        }

        private static string[] GetEntityTypeNames(IModel model)
        {
            return model?
                .GetEntityTypes()
                .Select(x => x.Name)
                .ToArray()
                   ?? Array.Empty<string>();
        }
        
        /// <inheritdoc />
        public override IReadOnlyList<MigrationOperation> GetDifferences(IRelationalModel source, IRelationalModel target)
        {
            var deleteTriggerOperations = new List<SqlOperation>();
            var createTriggerOperations = new List<SqlOperation>();

            var sourceModel = source?.Model;
            var targetModel = target?.Model;

            var oldEntityTypeNames = GetEntityTypeNames(sourceModel);
            var newEntityTypeNames = GetEntityTypeNames(targetModel);

            var commonEntityTypeNames = oldEntityTypeNames
                .Intersect(newEntityTypeNames)
                .ToArray();

            // Drop all triggers for deleted entities.
            foreach (var deletedTypeName in oldEntityTypeNames.Except(commonEntityTypeNames))
            {
                var deletedEntityType = source?.Model.FindEntityType(deletedTypeName);

                foreach (var annotation in deletedEntityType.GetTriggerAnnotations())
                {
                    deleteTriggerOperations.AddDeleteTriggerSqlMigration(annotation, sourceModel);
                }
            }

            // Add all triggers to created entities.
            foreach (var newTypeName in newEntityTypeNames.Except(commonEntityTypeNames))
            {
                foreach (var annotation in targetModel?.FindEntityType(newTypeName).GetTriggerAnnotations() ?? Array.Empty<IAnnotation>())
                {
                    createTriggerOperations.AddCreateTriggerSqlMigration(annotation);
                }
            }

            // For existing entities.
            foreach (var entityTypeName in commonEntityTypeNames)
            {
                var oldEntityType = sourceModel?.FindEntityType(entityTypeName);
                var newEntityType = targetModel?.FindEntityType(entityTypeName);

                var oldAnnotationNames = sourceModel?.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name)
                    .ToArray()
                        ?? Array.Empty<string>();

                var newAnnotationNames = targetModel?.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name)
                    .ToArray()
                        ?? Array.Empty<string>();

                var commonAnnotationNames = oldAnnotationNames
                    .Intersect(newAnnotationNames)
                    .ToArray();

                // If trigger was changed, recreate it.
                foreach (var commonAnnotationName in commonAnnotationNames)
                {
                    var oldValue = sourceModel?.FindEntityType(entityTypeName)?.GetAnnotation(commonAnnotationName);
                    var newValue = targetModel?.FindEntityType(entityTypeName)?.GetAnnotation(commonAnnotationName);

                    if ((string) oldValue?.Value == (string) newValue?.Value)
                    {
                        continue;
                    }
                    
                    deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldValue, sourceModel);
                    createTriggerOperations.AddCreateTriggerSqlMigration(newValue);
                }

                // If trigger was removed, delete it.
                foreach (var oldTriggerName in oldAnnotationNames.Except(commonAnnotationNames))
                {
                    var oldTriggerAnnotation = oldEntityType?.GetAnnotation(oldTriggerName);
                    
                    deleteTriggerOperations.AddDeleteTriggerSqlMigration(oldTriggerAnnotation, sourceModel);
                }

                // If trigger was added, create it.
                foreach (var newTriggerName in newAnnotationNames.Except(commonAnnotationNames))
                {
                    var newTriggerAnnotation = newEntityType?.GetAnnotation(newTriggerName);
                    
                    createTriggerOperations.AddCreateTriggerSqlMigration(newTriggerAnnotation);
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

    /// <summary>
    /// Extensions to create migrations for EF Core triggers.
    /// </summary>
    public static class MigrationsExtensions
    {
        /// <summary>
        /// Get all trigger annotations from <see cref="IEntityType"/>. 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IEnumerable<IAnnotation> GetTriggerAnnotations(this IEntityType entityType)
        {
            return entityType?.GetAnnotations()
                .Where(x => x.Name.StartsWith(Constants.AnnotationKey))
                    ?? Enumerable.Empty<IAnnotation>();
        }

        /// <summary>
        /// Adds sql operations necessary to create triggers.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public static IList<SqlOperation> AddCreateTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation)
        {
            if (annotation.Value is not string triggerSql)
            {
                return list;
            }

            list.Add(new SqlOperation 
            {
                Sql = triggerSql,
            });
            
            return list;
        }

        /// <summary>
        /// Adds sql operations necessary to drop triggers.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="annotation"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IList<SqlOperation> AddDeleteTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation, IReadOnlyModel model)
        {
            list.Add(new SqlOperation
            {
                Sql = TriggerExtensions.GetVisitor(model).GenerateDeleteTriggerSql(annotation.Name),
            });
            
            return list;
        }
    }
}