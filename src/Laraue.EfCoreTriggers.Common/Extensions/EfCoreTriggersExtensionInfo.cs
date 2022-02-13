using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.Common.Extensions;

/// <inheritdoc />
public class EfCoreTriggersExtensionInfo : DbContextOptionsExtensionInfo
{
    /// <summary>
    /// Initializes a new instance of <see cref="EfCoreTriggersExtensionInfo"/>.
    /// </summary>
    /// <param name="extension"></param>
    public EfCoreTriggersExtensionInfo(IDbContextOptionsExtension extension) 
        : base(extension)
    {
    }

    /// <inheritdoc />
    public override long GetServiceProviderHashCode()
    {
        return 0;
    }

    /// <inheritdoc />
    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
    {
    }

    /// <inheritdoc />
    public override bool IsDatabaseProvider => false;
    
    /// <inheritdoc />
    public override string LogFragment => "EfCoreTriggersExtension";
}