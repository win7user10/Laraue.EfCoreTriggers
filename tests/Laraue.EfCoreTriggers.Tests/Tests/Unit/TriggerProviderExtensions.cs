using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    public static class TriggerProviderExtensions
    {
        public static void AssertGeneratedInsertSql(
            this ITriggerActionVisitorFactory factory,
            string sql,
            Expression<Func<NewTableRef<SourceEntity>, DestinationEntity>> expression)
        {
            var trigger = new TriggerInsertAction(expression);

            var generatedSql = factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(sql, generatedSql);
        }
        
        public static void AssertGeneratedUpdateSql(
            this ITriggerActionVisitorFactory factory,
            string sql,
            Expression<Func<OldAndNewTableRefs<SourceEntity>, SourceEntity, DestinationEntity>> expression)
        {
            var trigger = new TriggerInsertAction(expression);

            var generatedSql = factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(sql, generatedSql);
        }
    }
}