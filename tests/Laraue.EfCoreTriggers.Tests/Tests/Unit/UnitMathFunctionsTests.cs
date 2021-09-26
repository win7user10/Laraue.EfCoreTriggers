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

        public abstract string ExceptedAbsSql { get; }

        public abstract string ExceptedAcosSql { get; }

        public abstract string ExceptedAsinSql { get; }

        public abstract string ExceptedAtanSql { get; }

        public abstract string ExceptedAtan2Sql { get; }

        public abstract string ExceptedCeilingSql { get; }

        public abstract string ExceptedCosSql { get; }

        public abstract string ExceptedExpSql { get; }

        public abstract string ExceptedFloorSql { get; }

        public override void MathAbsDecimalSql()
        {
            Provider.AssertGeneratedSql(ExceptedAbsSql, MathAbsDecimalValueExpression);
        }
        
        public override void MathAcosSql()
        {
            Provider.AssertGeneratedSql(ExceptedAcosSql, MathAcosDoubleValueExpression);
        }

        public override void MathAsinSql()
        {
            Provider.AssertGeneratedSql(ExceptedAsinSql, MathAsinDoubleValueExpression);
        }

        public override void MathAtanSql()
        {
            Provider.AssertGeneratedSql(ExceptedAtanSql, MathAtanDoubleValueExpression);
        }

        public override void MathAtan2Sql()
        {
            Provider.AssertGeneratedSql(ExceptedAtan2Sql, MathAtan2DoubleValueExpression);
        }

        public override void MathCeilingDoubleSql()
        {
            Provider.AssertGeneratedSql(ExceptedCeilingSql, MathCeilingDoubleValueExpression);
        }

        public override void MathCosSql()
        {
            Provider.AssertGeneratedSql(ExceptedCosSql, MathCosDoubleValueExpression);
        }

        public override void MathExpSql()
        {
            Provider.AssertGeneratedSql(ExceptedExpSql, MathExpDoubleValueExpression);
        }

        public override void MathFloorDoubleSql()
        {
            Provider.AssertGeneratedSql(ExceptedFloorSql, MathFloorDoubleValueExpression);
        }
    }
}