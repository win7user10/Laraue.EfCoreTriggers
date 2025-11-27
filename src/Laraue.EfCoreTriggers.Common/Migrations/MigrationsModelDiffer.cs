using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using BaseMigrationsModelDiffer = Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer;

namespace Laraue.EfCoreTriggers.Common.Migrations
{
    public class MigrationsModelDiffer : IMigrationsModelDiffer
    {
        private readonly ITriggerModelDiffer _triggerModelDiffer;
        private readonly BaseMigrationsModelDiffer _baseModelDiffer;
        
        public MigrationsModelDiffer(
            IServiceProvider serviceProvider,
            ITriggerModelDiffer triggerModelDiffer)
        {
            _triggerModelDiffer = triggerModelDiffer;
            _baseModelDiffer = ActivatorUtilities.CreateInstance<BaseMigrationsModelDiffer>(serviceProvider);
        }

        /// <inheritdoc />
        public bool HasDifferences(IRelationalModel? source, IRelationalModel? target)
        {
            return _baseModelDiffer.HasDifferences(source, target);
        }

        /// <inheritdoc />
        public IReadOnlyList<MigrationOperation> GetDifferences(IRelationalModel? source, IRelationalModel? target)
        {
            return _triggerModelDiffer.AddTriggerOperations(_baseModelDiffer.GetDifferences(source, target), source, target);
        }
    }
}