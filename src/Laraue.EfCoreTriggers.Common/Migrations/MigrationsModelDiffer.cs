using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Laraue.EfCoreTriggers.Common.Migrations
{
    /// <inheritdoc />
    public class MigrationsModelDiffer : Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer
    {
        private readonly TriggerModelDiffer _triggerModelDiffer;

#if NET6_0_OR_GREATER
        public MigrationsModelDiffer(
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotationProvider,
            IRowIdentityMapFactory rowIdentityMapFactory,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies,
            TriggerModelDiffer triggerModelDiffer) 
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
            TriggerModelDiffer triggerModelDiffer,
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