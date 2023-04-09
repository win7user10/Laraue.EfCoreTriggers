using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class UnitMathFunctionsTests : BaseMathFunctionsTests
    {
        protected readonly ITriggerActionVisitorFactory Factory;

        protected UnitMathFunctionsTests(ITriggerActionVisitorFactory factory)
        {
            Factory = factory;
        }

        protected abstract string ExceptedAbsSql { get; }

        protected abstract string ExceptedAcosSql { get; }

        protected abstract string ExceptedAsinSql { get; }

        protected abstract string ExceptedAtanSql { get; }

        protected abstract string ExceptedAtan2Sql { get; }

        protected abstract string ExceptedCeilingSql { get; }

        protected abstract string ExceptedCosSql { get; }

        protected abstract string ExceptedExpSql { get; }

        protected abstract string ExceptedFloorSql { get; }

        public override void MathAbsDecimalSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedAbsSql, MathAbsDecimalValueExpression);
        }
        
        public override void MathAcosSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedAcosSql, MathAcosDoubleValueExpression);
        }

        public override void MathAsinSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedAsinSql, MathAsinDoubleValueExpression);
        }

        public override void MathAtanSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedAtanSql, MathAtanDoubleValueExpression);
        }

        public override void MathAtan2Sql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedAtan2Sql, MathAtan2DoubleValueExpression);
        }

        public override void MathCeilingDoubleSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedCeilingSql, MathCeilingDoubleValueExpression);
        }

        public override void MathCosSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedCosSql, MathCosDoubleValueExpression);
        }

        public override void MathExpSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedExpSql, MathExpDoubleValueExpression);
        }

        public override void MathFloorDoubleSql()
        {
            Factory.AssertGeneratedInsertSql(ExceptedFloorSql, MathFloorDoubleValueExpression);
        }
    }
}