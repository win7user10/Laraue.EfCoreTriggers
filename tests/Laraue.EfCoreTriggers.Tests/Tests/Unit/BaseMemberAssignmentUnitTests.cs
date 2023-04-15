using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class BaseMemberAssignmentUnitTests : BaseMemberAssignmentTests
    {
        protected readonly ITriggerActionVisitorFactory Factory;

        protected BaseMemberAssignmentUnitTests(ITriggerActionVisitorFactory factory)
        {
            Factory = factory;
        }

        public abstract string ExceptedEnumValueSql { get; }

        public abstract string ExceptedDecimalAddSql { get; }

        public abstract string ExceptedDoubleSubSql { get; }

        public abstract string ExceptedIntMultiplySql { get; }

        public abstract string ExceptedBooleanSql { get; }

        public abstract string ExceptedNewGuidSql { get; }
        
        public abstract string ExceptedCharVariableSql { get; }
        
        public abstract string ExceptedCharValueSql { get; }
        
        public abstract string ExceptedNewDateTimeOffsetSql { get; }

        public override void EnumValueSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedEnumValueSql, SetEnumValueExpression);
        }

        public override void DecimalAddSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedDecimalAddSql, AddDecimalValueExpression);
        }

        public override void DoubleSubSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedDoubleSubSql, SubDoubleValueExpression);
        }

        public override void IntMultiplySql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedIntMultiplySql, MultiplyIntValueExpression);
        }

        public override void BooleanValueSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedBooleanSql, SetBooleanValueExpression);
        }

        public override void NewGuid()
        {
            Factory.AssertGeneratedInsertSql(ExceptedNewGuidSql, SetNewGuidValueExpression);
        }
        
        public override void CharVariableSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedCharVariableSql, SetCharVariableExpression);
        }

        public override void CharValueSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedCharValueSql, SetCharValueExpression);
        }

        public override void DateTimeOffsetValueSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedNewDateTimeOffsetSql, SetNewDateOffsetValueExpression);
        }
    }
}