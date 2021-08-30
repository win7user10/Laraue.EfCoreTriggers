using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    public static class TriggerProviderExtensions
    {
        public static void AssertGeneratedSql(this ITriggerProvider provider, string sql, Expression<Func<SourceEntity, DestinationEntity>> expression)
        {
            var trigger = new OnInsertTriggerInsertAction<SourceEntity, DestinationEntity>(expression);

            var generatedSql = trigger.BuildSql(provider);

            Assert.Equal(sql, generatedSql);
        }
    }
}