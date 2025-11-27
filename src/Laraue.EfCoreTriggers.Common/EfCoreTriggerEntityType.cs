using System;
using Laraue.Triggers.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laraue.EfCoreTriggers.Common;

/// <inheritdoc />
public class EfCoreTriggerEntityType : ITriggerEntityType
{
    private readonly IEntityType _entityType;

    /// <summary>
    /// Initializes a EF core wrapper for <see cref="ITriggerEntityType"/>.
    /// </summary>
    /// <param name="entityType"></param>
    public EfCoreTriggerEntityType(IEntityType entityType)
    {
        _entityType = entityType;
    }
    
    /// <inheritdoc />
    public Type ClrType => _entityType.ClrType;
    
    /// <inheritdoc />
    public string? GetSchema() => _entityType.GetSchema();
}