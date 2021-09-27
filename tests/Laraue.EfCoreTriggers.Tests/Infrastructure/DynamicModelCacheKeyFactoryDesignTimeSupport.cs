using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure
{
    /// <summary>
    /// Service to switch off DBContext caching.
    /// </summary>
    public class DynamicModelCacheKeyFactoryDesignTimeSupport : IModelCacheKeyFactory
    {
        private static int _invocationNum = 0;

        public object Create(DbContext context, bool designTime)
        {
            return _invocationNum++;
        }

        public object Create(DbContext context)
            => Create(context, false);
    }
}