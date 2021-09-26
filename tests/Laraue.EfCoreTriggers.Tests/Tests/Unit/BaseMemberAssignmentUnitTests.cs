using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class BaseMemberAssignmentUnitTests : BaseMemberAssignmentTests
    {
        protected readonly ITriggerProvider Provider;

        protected BaseMemberAssignmentUnitTests(ITriggerProvider provider)
        {
            Provider = provider;
        }

        public abstract string ExceptedEnumValueSql { get; }

        public abstract string ExceptedDecimalAddSql { get; }

        public abstract string ExceptedDoubleSubSql { get; }

        public abstract string ExceptedIntMultiplySql { get; }

        public abstract string ExceptedBooleanSql { get; }

        public abstract string ExceptedNewGuidSql { get; }

        public override void EnumValueSql()
        {
            Provider.AssertGeneratedSql(ExceptedEnumValueSql, SetEnumValueExpression);
        }

        public override void DecimalAddSql()
        {
            Provider.AssertGeneratedSql(ExceptedDecimalAddSql, AddDecimalValueExpression);
        }

        public override void DoubleSubSql()
        {
            Provider.AssertGeneratedSql(ExceptedDoubleSubSql, SubDoubleValueExpression);
        }

        public override void IntMultiplySql()
        {
            Provider.AssertGeneratedSql(ExceptedIntMultiplySql, MultiplyIntValueExpression);
        }

        public override void BooleanValueSql()
        {
            Provider.AssertGeneratedSql(ExceptedBooleanSql, SetBooleanValueExpression);
        }

        public override void NewGuid()
        {
            Provider.AssertGeneratedSql(ExceptedNewGuidSql, SetNewGuidValueExpression);
        }
    }
}