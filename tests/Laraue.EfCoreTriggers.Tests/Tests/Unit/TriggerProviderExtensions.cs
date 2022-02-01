using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    public static class TriggerProviderExtensions
    {
        public static void AssertGeneratedInsertSql(
            this ITriggerActionVisitorFactory factory,
            string sql,
            Expression<Func<SourceEntity, DestinationEntity>> expression)
        {
            var trigger = new OnInsertTriggerInsertAction<SourceEntity, DestinationEntity>(expression);

            var generatedSql = factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(sql, generatedSql);
        }
    }
}