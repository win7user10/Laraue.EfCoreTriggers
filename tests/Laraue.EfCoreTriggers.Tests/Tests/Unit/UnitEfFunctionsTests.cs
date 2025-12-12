using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit;

public abstract class UnitEfFunctionsTests : BaseEfFunctionsTests
{
    protected readonly ITriggerActionVisitorFactory Factory;

    protected UnitEfFunctionsTests(ITriggerActionVisitorFactory factory)
    {
        Factory = factory;
    }
    
    protected abstract string ExceptedEfPropertyTranslationSql { get; }

    public override void EfPropertyTranslationSql()
    {
        Factory.AssertGeneratedInsertSql(ExceptedEfPropertyTranslationSql, SetEfPropertyExpression);
    }
}