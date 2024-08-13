using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Base
{
    /// <summary>
    /// Tests of translating <see cref="string"/> functions to SQL code.
    /// </summary>
    public abstract class BaseGuidFunctionsTests
    {
        /// <summary>
        /// GuidValue = Guid.Empty
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> GuidEmptyExpression =
            sourceEntity => new DestinationEntity
            {
                GuidValue = Guid.Empty
            };

        [Fact]
        protected abstract void GuidEmptySql();

        /// <summary>
        /// GuidField = Guid.NewGuid()
        /// </summary>
        protected readonly Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> NewGuidExpression =
            sourceEntity => new DestinationEntity
            {
                GuidValue = Guid.NewGuid()
            };
        
        [Fact]
        protected abstract void NewGuidSql();
    }
}