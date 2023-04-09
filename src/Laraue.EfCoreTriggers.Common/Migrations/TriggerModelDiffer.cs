using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ITrigger = Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions.ITrigger;

namespace Laraue.EfCoreTriggers.Common.Migrations
{
    /// <inheritdoc />
    public class TriggerModelDiffer : ITriggerModelDiffer
    {
        private readonly ITriggerVisitor _triggerVisitor;

        /// <summary>
        /// Initializes a new instance of <see cref="TriggerModelDiffer"/>.
        /// </summary>
        /// <param name="triggerVisitor"></param>
        public TriggerModelDiffer(ITriggerVisitor triggerVisitor) 
        {
            _triggerVisitor = triggerVisitor;
        }
        
        /// <inheritdoc />
        public IReadOnlyList<MigrationOperation> AddTriggerOperations(
            IEnumerable<MigrationOperation> operations,
            IRelationalModel? source,
            IRelationalModel? target)
        {
            var deleteTriggerOperations = new List<SqlOperation>();
            var createTriggerOperations = new List<SqlOperation>();

            var sourceModel = source?.Model;
            var targetModel = target?.Model;
            
            _triggerVisitor.ConvertTriggerAnnotationsToSql(sourceModel);
            _triggerVisitor.ConvertTriggerAnnotationsToSql(targetModel);

            var oldEntityTypeNames = sourceModel.GetEntityTypeNames();
            var newEntityTypeNames = targetModel.GetEntityTypeNames();

            var commonEntityTypeNames = oldEntityTypeNames
                .Intersect(newEntityTypeNames)
                .ToArray();

            // Drop all triggers for deleted entities.
            foreach (var deletedTypeName in oldEntityTypeNames.Except(commonEntityTypeNames))
            {
                var deletedEntityType = sourceModel?.FindEntityType(deletedTypeName);

                foreach (var annotation in deletedEntityType?.GetTriggerAnnotations() ?? Array.Empty<IAnnotation>())
                {
                    _triggerVisitor.AddDeleteTriggerSqlMigration(deleteTriggerOperations, annotation, deletedEntityType!);
                }
            }

            // Add all triggers to created entities.
            foreach (var newTypeName in newEntityTypeNames.Except(commonEntityTypeNames))
            {
                foreach (var annotation in targetModel?.FindEntityType(newTypeName)?.GetTriggerAnnotations() ?? Array.Empty<IAnnotation>())
                {
                    createTriggerOperations.AddCreateTriggerSqlMigration(annotation);
                }
            }
            
            // For existing entities.
            foreach (var entityTypeName in commonEntityTypeNames)
            {
                var oldEntityType = sourceModel!.FindEntityType(entityTypeName);
                var newEntityType = targetModel!.FindEntityType(entityTypeName);

                var oldAnnotationNames = sourceModel.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name)
                    .ToArray();

                var newAnnotationNames = targetModel.FindEntityType(entityTypeName)
                    .GetTriggerAnnotations()
                    .Select(x => x.Name)
                    .ToArray();

                var commonAnnotationNames = oldAnnotationNames
                    .Intersect(newAnnotationNames)
                    .ToArray();

                // If trigger was changed, recreate it.
                foreach (var commonAnnotationName in commonAnnotationNames)
                {
                    var oldValue = sourceModel?.FindEntityType(entityTypeName)?.GetAnnotation(commonAnnotationName)!;
                    var newValue = targetModel?.FindEntityType(entityTypeName)?.GetAnnotation(commonAnnotationName)!;
                    
                    if (oldValue.Value?.ToString() == newValue.Value?.ToString())
                    {
                        continue;
                    }
                    
                    _triggerVisitor.AddDeleteTriggerSqlMigration(
                        deleteTriggerOperations,
                        oldValue,
                        newEntityType);
                    
                    createTriggerOperations.AddCreateTriggerSqlMigration(newValue);
                }

                // If trigger was removed, delete it.
                foreach (var oldTriggerName in oldAnnotationNames.Except(commonAnnotationNames))
                {
                    var oldTriggerAnnotation = oldEntityType.GetAnnotation(oldTriggerName);

                    _triggerVisitor.AddDeleteTriggerSqlMigration(
                        deleteTriggerOperations,
                        oldTriggerAnnotation,
                        newEntityType);
                }

                // If trigger was added, create it.
                foreach (var newTriggerName in newAnnotationNames.Except(commonAnnotationNames))
                {
                    var newTriggerAnnotation = newEntityType.GetAnnotation(newTriggerName);

                    createTriggerOperations.AddCreateTriggerSqlMigration(newTriggerAnnotation);
                }
            }

            return MergeOperations(operations, createTriggerOperations, deleteTriggerOperations);
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
#if NET6_0_OR_GREATER
        private static readonly FieldInfo AnnotationsField = typeof(AnnotatableBase)
            .GetField("_annotations", BindingFlags.Instance | BindingFlags.NonPublic)!;
#else
        private static readonly FieldInfo AnnotationsField = typeof(Annotatable)
            .GetField("_annotations", BindingFlags.Instance | BindingFlags.NonPublic)!;
#endif
        
        
        /// <summary>
        /// Convert all not translated annotations of <see cref="Microsoft.EntityFrameworkCore.Metadata.ITrigger"/> type to SQL.
        /// </summary>
        /// <param name="triggerVisitor"></param>
        /// <param name="model"></param>
        public static void ConvertTriggerAnnotationsToSql(this ITriggerVisitor triggerVisitor, IModel? model)
        {
            foreach (var entityType in model?.GetEntityTypes() ?? Enumerable.Empty<IEntityType>())
            {
                var annotations = (SortedDictionary<string, Annotation>?) AnnotationsField.GetValue(entityType);

                if (annotations is null)
                {
                    return;
                }

                foreach (var key in annotations.Keys.ToArray())
                {
                    if (!key.StartsWith(Constants.AnnotationKey))
                    {
                        continue;
                    }
                    
                    var annotation = annotations[key];

                    var value = annotation.Value;

                    if (value is not ITrigger trigger)
                    {
                        continue;
                    }
                    
                    var sql = triggerVisitor.GenerateCreateTriggerSql(trigger);
                    annotations[key] = new ConventionAnnotation(key, sql, ConfigurationSource.DataAnnotation);
                }
            }
        }
        
        /// <summary>
        /// Get names of entities in <see cref="IModel"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string[] GetEntityTypeNames(this IModel? model)
        {
            return model?
                .GetEntityTypes()
                .Select(x => x.Name)
                .ToArray()
                   ?? Array.Empty<string>();
        }
        
        /// <summary>
        /// Get all trigger annotations from <see cref="IEntityType"/>. 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IEnumerable<IAnnotation> GetTriggerAnnotations(this IEntityType? entityType)
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
        public static void AddCreateTriggerSqlMigration(this IList<SqlOperation> list, IAnnotation annotation)
        {
            if (annotation.Value is not string createSql)
            {
                return;
            }

            list.Add(new SqlOperation
            {
                Sql = createSql,
            });
        }

        /// <summary>
        /// Adds sql operations necessary to drop triggers.
        /// </summary>
        /// <param name="triggerVisitor"></param>
        /// <param name="list"></param>
        /// <param name="annotation"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static void AddDeleteTriggerSqlMigration(
            this ITriggerVisitor triggerVisitor,
            IList<SqlOperation> list,
            IAnnotation annotation,
            IEntityType entityType)
        {
            list.Add(new SqlOperation
            {
                Sql = triggerVisitor.GenerateDeleteTriggerSql(annotation.Name, entityType),
            });
        }
    }
}