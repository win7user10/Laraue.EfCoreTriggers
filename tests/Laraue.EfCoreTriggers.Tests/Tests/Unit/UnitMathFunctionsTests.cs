using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Tests.Tests.Base;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.Tests.Tests.Unit
{
    [UnitTest]
    public abstract class UnitMathFunctionsTests : BaseMathFunctionsTests
    {
        protected readonly ITriggerProvider Provider;

        protected UnitMathFunctionsTests(ITriggerProvider provider)
        {
            Provider = provider;
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
            Provider.AssertGeneratedInsertSql(ExceptedAbsSql, MathAbsDecimalValueExpression);
        }
        
        public override void MathAcosSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedAcosSql, MathAcosDoubleValueExpression);
        }

        public override void MathAsinSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedAsinSql, MathAsinDoubleValueExpression);
        }

        public override void MathAtanSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedAtanSql, MathAtanDoubleValueExpression);
        }

        public override void MathAtan2Sql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedAtan2Sql, MathAtan2DoubleValueExpression);
        }

        public override void MathCeilingDoubleSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedCeilingSql, MathCeilingDoubleValueExpression);
        }

        public override void MathCosSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedCosSql, MathCosDoubleValueExpression);
        }

        public override void MathExpSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedExpSql, MathExpDoubleValueExpression);
        }

        public override void MathFloorDoubleSql()
        {
            Provider.AssertGeneratedInsertSql(ExceptedFloorSql, MathFloorDoubleValueExpression);
        }
    }
}