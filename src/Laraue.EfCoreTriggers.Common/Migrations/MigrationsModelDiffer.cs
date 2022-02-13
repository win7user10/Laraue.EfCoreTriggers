using System;
using System.Collections.Generic;
using System.Linq;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
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
    /// <inheritdoc />
    public class MigrationsModelDiffer : Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer
    {
        private readonly ITriggerVisitor _triggerVisitor;

        /// <inheritdoc />
        public MigrationsModelDiffer(
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotations,
            IChangeDetector changeDetector,
            IUpdateAdapterFactory updateAdapterFactory,
            ITriggerVisitor triggerVisitor,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies)
                : base (typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory, commandBatchPreparerDependencies)
        {
            _triggerVisitor = triggerVisitor;
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
                    _triggerVisitor.AddDeleteTriggerSqlMigration(deleteTriggerOperations, annotation, sourceModel);
                }
            }

            // Add all triggers to created entities.
            foreach (var newTypeName in newEntityTypeNames.Except(commonEntityTypeNames))
            {
                foreach (var annotation in targetModel?.FindEntityType(newTypeName).GetTriggerAnnotations() ?? Array.Empty<IAnnotation>())
                {
                    _triggerVisitor.AddCreateTriggerSqlMigration(createTriggerOperations, annotation);
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

                    var oldTrigger = _triggerVisitor.ConvertTriggerToSql(oldValue);
                    var newTrigger = _triggerVisitor.ConvertTriggerToSql(newValue);
                    if (oldTrigger == newTrigger)
                    {
                        continue;
                    }
                    
                    _triggerVisitor.AddDeleteTriggerSqlMigration(deleteTriggerOperations, oldValue, sourceModel);
                    _triggerVisitor.AddCreateTriggerSqlMigration(createTriggerOperations, newValue);
                }

                // If trigger was removed, delete it.
                foreach (var oldTriggerName in oldAnnotationNames.Except(commonAnnotationNames))
                {
                    var oldTriggerAnnotation = oldEntityType?.GetAnnotation(oldTriggerName);

                    _triggerVisitor.AddDeleteTriggerSqlMigration(deleteTriggerOperations, oldTriggerAnnotation, sourceModel);
                }

                // If trigger was added, create it.
                foreach (var newTriggerName in newAnnotationNames.Except(commonAnnotationNames))
                {
                    var newTriggerAnnotation = newEntityType?.GetAnnotation(newTriggerName);

                    _triggerVisitor.AddCreateTriggerSqlMigration(createTriggerOperations, newTriggerAnnotation);
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
        public static string ConvertTriggerToSql(this ITriggerVisitor visitor, IAnnotation annotation)
        {
            return annotation.Value switch
            {
                ITrigger trigger => visitor.GenerateCreateTriggerSql(trigger),
                string str => str,
                _ => throw new InvalidOperationException($"The annotation of the type {annotation.Value.GetType()} can not be translated to SQL")
            };
        }
        
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
        /// <param name="triggerVisitor"></param>
        /// <param name="list"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public static IList<SqlOperation> AddCreateTriggerSqlMigration(this ITriggerVisitor triggerVisitor, IList<SqlOperation> list, IAnnotation annotation)
        {
            var triggerSql = triggerVisitor.ConvertTriggerToSql(annotation);
            
            if (triggerSql is null)
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
        /// <param name="triggerVisitor"></param>
        /// <param name="list"></param>
        /// <param name="annotation"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IList<SqlOperation> AddDeleteTriggerSqlMigration(this ITriggerVisitor triggerVisitor, IList<SqlOperation> list, IAnnotation annotation, IModel model)
        {
            list.Add(new SqlOperation
            {
                Sql = triggerVisitor.GenerateDeleteTriggerSql(annotation.Name),
            });
            
            return list;
        }
    }
}