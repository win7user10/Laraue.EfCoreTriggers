using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base
{
    /// <summary>
    /// Tests of translating <see cref="DateTime"/> functions to SQL code.
    /// </summary>
    public abstract class BaseDateTimeFunctionsTests
    {
        /// <summary>
        /// DateTimeValue = DateTime.UtcNow
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> DateTimeUtcNowExpression =
            tableRefs => new DestinationEntity
            {
                DateTimeValue = DateTime.UtcNow
            };

        [Fact]
        public abstract void DateTimeUtcNowSql();
        
        /// <summary>
        /// DateTimeValue = DateTime.Now
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> DateTimeNowExpression =
            tableRefs => new DestinationEntity
            {
                DateTimeValue = DateTime.Now
            };

        [Fact]
        public abstract void DateTimeNowSql();
    }
}