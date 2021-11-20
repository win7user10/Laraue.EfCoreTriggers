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
            Provider.AssertGeneratedInsertSql(ExceptedEnumValueSql, SetEnumValueExpression);
        }

        public override void DecimalAddSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedDecimalAddSql, AddDecimalValueExpression);
        }

        public override void DoubleSubSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedDoubleSubSql, SubDoubleValueExpression);
        }

        public override void IntMultiplySql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedIntMultiplySql, MultiplyIntValueExpression);
        }

        public override void BooleanValueSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedBooleanSql, SetBooleanValueExpression);
        }

        public override void NewGuid()
        {
            Provider.AssertGeneratedInsertSql(ExceptedNewGuidSql, SetNewGuidValueExpression);
        }
    }
}