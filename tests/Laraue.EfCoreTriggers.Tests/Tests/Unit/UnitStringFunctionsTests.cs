using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class UnitStringFunctionsTests : BaseStringFunctionsTests
    {
        protected readonly ITriggerActionVisitorFactory Factory;

        protected UnitStringFunctionsTests(ITriggerActionVisitorFactory factory)
        {
            Factory = factory;
        }

        public abstract string ExceptedConcatSql { get; }

        public abstract string ExceptedStringLowerSql { get; }

        public abstract string ExceptedStringUpperSql { get; }

        public abstract string ExceptedStringTrimSql { get; }

        public abstract string ExceptedContainsSql { get; }

        public abstract string ExceptedEndsWithSql { get; }

        public abstract string ExceptedIsNullOrEmptySql { get; }

        protected override void StringConcatSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedConcatSql, ConcatStringExpression);
        }

        protected override void StringLowerSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedStringLowerSql, StringToLowerExpression);
        }

        protected override void StringUpperSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedStringUpperSql, StringToUpperExpression);
        }

        protected override void StringTrimSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedStringTrimSql, TrimStringValueExpression);
        }

        protected override void StringContainsSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedContainsSql, ContainsStringValueExpression);
        }
        
        protected override void StringEndsWithSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedEndsWithSql, EndsWithStringValueExpression);
        }

        protected override void StringIsNullOrEmptySql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedIsNullOrEmptySql, IsNullOrEmptyStringValueExpression);
        }
    }
}