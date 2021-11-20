using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.OnInsert;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class UnitRawSqlTests
    {
        protected readonly ITriggerProvider Provider;

        protected UnitRawSqlTests(ITriggerProvider provider)
        {
            Provider = provider;
        }
        
        protected abstract string ExceptedSqlForMemberArgs { get; }
        
        [Fact]
        protected void GenerateSqlForMemberArgs()
        {
            Expression<Func<SourceEntity, object>> arg1Expression = sourceEntity => sourceEntity.BooleanValue;
            Expression<Func<SourceEntity, object>> arg2Expression = sourceEntity => sourceEntity.DoubleValue;
            
            var trigger = new OnInsertTriggerRawAction<SourceEntity>("PERFORM func({0}, {1})", arg1Expression, arg2Expression);

            var generatedSql = trigger.BuildSql(Provider);

            Assert.Equal(ExceptedSqlForMemberArgs, generatedSql);
        }
        
        
        protected abstract string ExceptedSqlForComputedArgs { get; }
        
        [Fact]
        protected void GenerateSqlForComputedArgs()
        {
            Expression<Func<SourceEntity, object>> argExpression = sourceEntity => sourceEntity.DoubleValue + 10;
            
            var trigger = new OnInsertTriggerRawAction<SourceEntity>("PERFORM func({0})", argExpression);

            var generatedSql = trigger.BuildSql(Provider);

            Assert.Equal(ExceptedSqlForComputedArgs, generatedSql);
        }
        
        protected abstract string ExceptedSqlWhenNoArgs { get; }
        
        [Fact]
        protected void GenerateSqlWhenNoArgs()
        {
            var trigger = new OnInsertTriggerRawAction<SourceEntity>("PERFORM func()");

            var generatedSql = trigger.BuildSql(Provider);

            Assert.Equal(ExceptedSqlWhenNoArgs, generatedSql);
        }
    }
}