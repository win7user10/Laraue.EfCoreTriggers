using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.SqlServerTests 
{
    [UnitTest]
    public class SqlServerUnitMathFunctionsTest : UnitMathFunctionsTests
    {
        public SqlServerUnitMathFunctionsTest() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedAbsSql => "INSERT INTO transactions_mirror (value) VALUES (ABS(@NewValue));";

        public override string ExceptedAcosSql => "INSERT INTO transactions_mirror (double_value) VALUES (ACOS(@NewDoubleValue));";

        public override string ExceptedAsinSql => "INSERT INTO transactions_mirror (double_value) VALUES (ASIN(@NewDoubleValue));";

        public override string ExceptedAtanSql => "INSERT INTO transactions_mirror (double_value) VALUES (ATAN(@NewDoubleValue));";

        public override string ExceptedAtan2Sql => "INSERT INTO transactions_mirror (double_value) VALUES (ATAN2(@NewDoubleValue, @NewDoubleValue));";

        public override string ExceptedCeilingSql => "INSERT INTO transactions_mirror (double_value) VALUES (CEILING(@NewDoubleValue));";

        public override string ExceptedCosSql => "INSERT INTO transactions_mirror (double_value) VALUES (COS(@NewDoubleValue));";

        public override string ExceptedExpSql => "INSERT INTO transactions_mirror (double_value) VALUES (EXP(@NewDoubleValue));";

        public override string ExceptedFloorSql => "INSERT INTO transactions_mirror (double_value) VALUES (FLOOR(@NewDoubleValue));";

    }
}
