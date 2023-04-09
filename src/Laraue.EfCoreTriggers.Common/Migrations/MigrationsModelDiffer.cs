using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Laraue.EfCoreTriggers.Common.Migrations
{
    /// <inheritdoc />
    public class MigrationsModelDiffer : Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer
    {
        private readonly ITriggerModelDiffer _triggerModelDiffer;

        /// <summary>
        /// Initializes a new instance of <see cref="MigrationsModelDiffer"/>.
        /// </summary>
        /// <param name="typeMappingSource"></param>
        /// <param name="migrationsAnnotations"></param>
        /// <param name="changeDetector"></param>
        /// <param name="updateAdapterFactory"></param>
        /// <param name="triggerModelDiffer"></param>
        /// <param name="commandBatchPreparerDependencies"></param>
#if NET6_0_OR_GREATER
        public MigrationsModelDiffer(
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotationProvider,
            IRowIdentityMapFactory rowIdentityMapFactory,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies,
            ITriggerModelDiffer triggerModelDiffer) 
            : base(typeMappingSource, migrationsAnnotationProvider, rowIdentityMapFactory, commandBatchPreparerDependencies)
        {
            _triggerModelDiffer = triggerModelDiffer;
        }
#else
        public MigrationsModelDiffer(
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotations,
            IChangeDetector changeDetector,
            IUpdateAdapterFactory updateAdapterFactory,
            ITriggerModelDiffer triggerModelDiffer,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies)
            : base (typeMappingSource, migrationsAnnotations, changeDetector, updateAdapterFactory, commandBatchPreparerDependencies)
        {
            _triggerModelDiffer = triggerModelDiffer;
        }
#endif

        /// <inheritdoc />
        public override IReadOnlyList<MigrationOperation> GetDifferences(IRelationalModel? source, IRelationalModel? target)
        {
            return _triggerModelDiffer.AddTriggerOperations(base.GetDifferences(source, target), source, target);
        }
    }
}