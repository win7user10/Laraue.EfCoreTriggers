using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit;

public abstract class UnitEnumerableFunctionsTests : BaseEnumerableFunctionsTests
{
    protected readonly ITriggerActionVisitorFactory Factory;

    protected UnitEnumerableFunctionsTests(ITriggerActionVisitorFactory factory)
    {
        Factory = factory;
    }
    
    protected abstract string ExceptedCountRelatedSql { get; }

    [Fact]
    public override void CountRelatedSql()
    {
        Factory.AssertGeneratedUpdateSql(ExceptedCountRelatedSql, CountRelatedExpression);
    }
    
    protected abstract string ExceptedCountRelatedWithPredicateSql { get; }

    [Fact]
    public override void CountRelatedWithPredicateSql()
    {
        Factory.AssertGeneratedUpdateSql(ExceptedCountRelatedSql, CountRelatedWithPredicateExpression);
    }
}