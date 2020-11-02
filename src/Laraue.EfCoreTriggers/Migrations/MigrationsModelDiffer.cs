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

        public override IReadOnlyList<MigrationOperation> GetDifferences(IModel source, IModel target)
        {
            var differences = new List<MigrationOperation>();

            var oldEntityTypeNames = source?.GetEntityTypes().Select(x => x.Name) ?? Enumerable.Empty<string>();
            var newEntityTypeNames = target?.GetEntityTypes().Select(x => x.Name) ?? Enumerable.Empty<string>();

            var commonEntityTypeNames = oldEntityTypeNames.Intersect(newEntityTypeNames);

            // Drop all triggers for deleted entities.
            foreach (var deletedTypeName in oldEntityTypeNames.Except(commonEntityTypeNames))
            {
                var deletedEntityType = source.FindEntityType(deletedTypeName);
                deletedEntityType.GetTriggerAnnotations()
                    .SafeForEach(annotation => differences.AddDeleteTriggerSqlMigration(annotation, deletedEntityType.ClrType, source));
            }

            // Add all triggers to created entities.
            foreach (var newTypeName in newEntityTypeNames.Except(commonEntityTypeNames))
            {
                target.FindEntityType(newTypeName).GetTriggerAnnotations()
                    .SafeForEach(annotation => differences.AddCreateTriggerSqlMigration(annotation));
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
                        differences.AddDeleteTriggerSqlMigration(oldValue, clrType, source);
                        differences.AddCreateTriggerSqlMigration(newValue);
                    }
                }

                // If trigger was removed, delete it.
                foreach (var oldTriggerName in oldAnnotationNames.Except(commonAnnotationNames))
                {
                    var oldTriggerAnnotation = oldEntityType.GetAnnotation(oldTriggerName);
                    differences.AddDeleteTriggerSqlMigration(oldTriggerAnnotation, clrType, source);
                }

                // If trigger was added, create it.
                foreach (var newTriggerName in newAnnotationNames.Except(commonAnnotationNames))
                {
                    var newTriggerAnnotation = newEntityType.GetAnnotation(newTriggerName);
                    differences.AddCreateTriggerSqlMigration(newTriggerAnnotation);
                }
            }
            
            return differences.Concat(base.GetDifferences(source, target)).ToList();
        }
    }

    public static class IListExtensions
    {
        public static IEnumerable<IAnnotation> GetTriggerAnnotations(this IEntityType entityType)
        {
            return entityType.GetAnnotations()
                .Where(x => x.Name.StartsWith(Constants.AnnotationKey));
        }

        public static IList<MigrationOperation> AddCreateTriggerSqlMigration(this IList<MigrationOperation> list, IAnnotation annotation)
        {
            list.Add(new SqlOperation 
            {
                Sql = annotation.Value.ToString(),
            });
            return list;
        }

        public static IList<MigrationOperation> AddDeleteTriggerSqlMigration(this IList<MigrationOperation> list, IAnnotation annotation, Type entityType, IModel model)
        {
            list.Add(new SqlOperation
            {
                Sql = Initializer.GetSqlProvider(model).GetDropTriggerSql(annotation.Name, entityType),
            });
            return list;
        }
    }
}