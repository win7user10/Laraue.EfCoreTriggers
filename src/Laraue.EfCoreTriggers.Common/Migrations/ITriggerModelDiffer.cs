using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Laraue.EfCoreTriggers.Common.Migrations;

/// <summary>
/// Sql operations generator for the difference between two database models.
/// </summary>
public interface ITriggerModelDiffer
{
    /// <summary>
    /// Add trigger migration operations to the list of migration operations.
    /// </summary>
    /// <param name="operations"></param>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public IReadOnlyList<MigrationOperation> AddTriggerOperations(
        IEnumerable<MigrationOperation> operations,
        IRelationalModel? source,
        IRelationalModel? target);
}