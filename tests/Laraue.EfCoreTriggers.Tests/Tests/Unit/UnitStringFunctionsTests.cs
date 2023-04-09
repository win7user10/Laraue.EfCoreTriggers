using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class UnitStringFunctionsTests : BaseStringFunctionsTests
    {
        private readonly ITriggerActionVisitorFactory _factory;

        protected UnitStringFunctionsTests(ITriggerActionVisitorFactory factory)
        {
            _factory = factory;
        }

        protected abstract string ExceptedConcatSql { get; }

        protected abstract string ExceptedStringLowerSql { get; }

        protected abstract string ExceptedStringUpperSql { get; }

        protected abstract string ExceptedStringTrimSql { get; }

        protected abstract string ExceptedContainsSql { get; }

        protected abstract string ExceptedEndsWithSql { get; }

        protected abstract string ExceptedIsNullOrEmptySql { get; }

        protected abstract string ExceptedCoalesceStringSql { get; }

        protected override void StringConcatSql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedConcatSql, ConcatStringExpression);
        }

        protected override void StringLowerSql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedStringLowerSql, StringToLowerExpression);
        }

        protected override void StringUpperSql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedStringUpperSql, StringToUpperExpression);
        }

        protected override void StringTrimSql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedStringTrimSql, TrimStringValueExpression);
        }

        protected override void StringContainsSql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedContainsSql, ContainsStringValueExpression);
        }
        
        protected override void StringEndsWithSql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedEndsWithSql, EndsWithStringValueExpression);
        }

        protected override void StringIsNullOrEmptySql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedIsNullOrEmptySql, IsNullOrEmptyStringValueExpression);
        }

        protected override void CoalesceStringSql()
        {
            _factory.AssertGeneratedInsertSql(ExceptedCoalesceStringSql, CoalesceStringExpression);
        }
    }
}