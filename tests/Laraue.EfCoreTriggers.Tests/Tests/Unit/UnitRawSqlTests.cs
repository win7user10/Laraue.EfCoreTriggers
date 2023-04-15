using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Functions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class UnitRawSqlTests
    {
        private readonly ITriggerActionVisitorFactory _factory;

        protected UnitRawSqlTests(ITriggerActionVisitorFactory factory)
        {
            _factory = factory;
        }
        
        protected abstract string ExceptedInsertTriggerSqlForMemberArgs { get; }
        
        [Fact]
        protected void GenerateSqlForMemberArgs()
        {
            Expression<Func<NewTableRef<SourceEntity>, object>> arg1Expression = sourceEntity => sourceEntity.New.BooleanValue;
            Expression<Func<NewTableRef<SourceEntity>, object>> arg2Expression = sourceEntity => sourceEntity.New.DoubleValue;
            Expression<Func<NewTableRef<SourceEntity>, object>> arg3Expression = sourceEntity => TriggerFunctions.GetTableName<SourceEntity>();
            
            var trigger = new TriggerRawAction(
                "PERFORM func({0}, {1}, {2})",
                arg1Expression, 
                arg2Expression,
                arg3Expression);

            var generatedSql = _factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(ExceptedInsertTriggerSqlForMemberArgs, generatedSql);
        }
        
        
        protected abstract string ExceptedInsertTriggerSqlForComputedArgs { get; }
        
        [Fact]
        protected void GenerateSqlForComputedArgs()
        {
            Expression<Func<NewTableRef<SourceEntity>, object>> argExpression = sourceEntity
                => sourceEntity.New.DoubleValue + 10;
            
            var trigger = new TriggerRawAction("PERFORM func({0})", argExpression);

            var generatedSql = _factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(ExceptedInsertTriggerSqlForComputedArgs, generatedSql);
        }
        
        protected abstract string ExceptedInsertTriggerSqlWhenNoArgs { get; }
        
        [Fact]
        protected void GenerateSqlWhenNoArgs()
        {
            var trigger = new TriggerRawAction("PERFORM func()");

            var generatedSql = _factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(ExceptedInsertTriggerSqlWhenNoArgs, generatedSql);
        }
        
        protected abstract string ExceptedUpdateTriggerSqlForMemberArgs { get; }
        
        [Fact]
        protected void GenerateSqlForUpdateTrigger()
        {
            Expression<Func<OldAndNewTableRefs<SourceEntity>, object>> arg1Expression = tableRefs
                => tableRefs.Old.DecimalValue;
            
            Expression<Func<OldAndNewTableRefs<SourceEntity>, object>> arg2Expression = tableRefs
                => tableRefs.New.DecimalValue;
            
            var trigger = new TriggerRawAction(
                "PERFORM func({0}, {1})",
                arg1Expression,
                arg2Expression);

            var generatedSql = _factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(ExceptedUpdateTriggerSqlForMemberArgs, generatedSql);
        }
        
        protected abstract string ExceptedDeleteTriggerSqlForMemberArgs { get; }
        
        [Fact]
        protected void GenerateSqlForDeleteTrigger()
        {
            Expression<Func<OldTableRef<SourceEntity>, object>> arg1Expression = tableRefs
                => tableRefs.Old.DecimalValue;
            
            Expression<Func<OldTableRef<SourceEntity>, object>> arg2Expression = tableRefs
                => tableRefs.Old.DoubleValue;
            
            var trigger = new TriggerRawAction(
                "PERFORM func({0}, {1})",
                arg1Expression,
                arg2Expression);

            var generatedSql = _factory.Visit(trigger, new VisitedMembers());

            Assert.Equal(ExceptedDeleteTriggerSqlForMemberArgs, generatedSql);
        }
    }
}