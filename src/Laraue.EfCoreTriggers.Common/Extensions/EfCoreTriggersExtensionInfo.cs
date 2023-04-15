using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
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
#if NET6_0_OR_GREATER
    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
    {
        return string.Equals(LogFragment, other.LogFragment, StringComparison.Ordinal);
    }
    public override int GetServiceProviderHashCode() => 0;
#else
        public override long GetServiceProviderHashCode() => 0;
#endif

        /// <inheritdoc />
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        /// <inheritdoc />
        public override bool IsDatabaseProvider => false;
    
        /// <inheritdoc />
        public override string LogFragment => "EfCoreTriggersExtension";
    }
}