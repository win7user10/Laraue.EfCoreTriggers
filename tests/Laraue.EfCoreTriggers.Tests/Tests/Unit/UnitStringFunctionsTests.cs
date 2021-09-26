using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class UnitStringFunctionsTests : BaseStringFunctionsTests
    {
        protected readonly ITriggerProvider Provider;

        protected UnitStringFunctionsTests(ITriggerProvider provider)
        {
            Provider = provider;
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
            Provider.AssertGeneratedSql(ExceptedConcatSql, ConcatStringExpression);
        }

        protected override void StringLowerSql()
        {
            Provider.AssertGeneratedSql(ExceptedStringLowerSql, StringToLowerExpression);
        }

        protected override void StringUpperSql()
        {
            Provider.AssertGeneratedSql(ExceptedStringUpperSql, StringToUpperExpression);
        }

        protected override void StringTrimSql()
        {
            Provider.AssertGeneratedSql(ExceptedStringTrimSql, TrimStringValueExpression);
        }

        protected override void StringContainsSql()
        {
            Provider.AssertGeneratedSql(ExceptedContainsSql, ContainsStringValueExpression);
        }
        
        protected override void StringEndsWithSql()
        {
            Provider.AssertGeneratedSql(ExceptedEndsWithSql, EndsWithStringValueExpression);
        }

        protected override void StringIsNullOrEmptySql()
        {
            Provider.AssertGeneratedSql(ExceptedIsNullOrEmptySql, IsNullOrEmptyStringValueExpression);
        }
    }
}