using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Tests.Base;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit;

public abstract class UnitDateTimeFunctionsTests : BaseDateTimeFunctionsTests
{
    protected readonly ITriggerActionVisitorFactory Factory;

    protected UnitDateTimeFunctionsTests(ITriggerActionVisitorFactory factory)
    {
        Factory = factory;
    }
    
    protected abstract string ExceptedDateTimeUtcNowSql { get; }

    public override void DateTimeUtcNowSql()
    {
        Factory.AssertGeneratedInsertSql(ExceptedDateTimeUtcNowSql, DateTimeUtcNowExpression);
    }

    protected abstract string ExceptedDateTimeNowSql { get; }
    
    public override void DateTimeNowSql()
    {
        Factory.AssertGeneratedInsertSql(ExceptedDateTimeNowSql, DateTimeNowExpression);
    }
}