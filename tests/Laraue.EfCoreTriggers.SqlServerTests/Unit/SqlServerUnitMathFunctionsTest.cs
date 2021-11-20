using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit 
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitMathFunctionsTest : UnitMathFunctionsTests
    {
        public SqlServerUnitMathFunctionsTest() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        protected override string ExceptedAbsSql => "INSERT INTO destination_entities (\"decimal_value\") VALUES (ABS(@NewDecimalValue));";

        protected override string ExceptedAcosSql => "INSERT INTO destination_entities (\"double_value\") VALUES (ACOS(@NewDoubleValue));";

        protected override string ExceptedAsinSql => "INSERT INTO destination_entities (\"double_value\") VALUES (ASIN(@NewDoubleValue));";

        protected override string ExceptedAtanSql => "INSERT INTO destination_entities (\"double_value\") VALUES (ATAN(@NewDoubleValue));";

        protected override string ExceptedAtan2Sql => "INSERT INTO destination_entities (\"double_value\") VALUES (ATN2(@NewDoubleValue, @NewDoubleValue));";

        protected override string ExceptedCeilingSql => "INSERT INTO destination_entities (\"double_value\") VALUES (CEILING(@NewDoubleValue));";

        protected override string ExceptedCosSql => "INSERT INTO destination_entities (\"double_value\") VALUES (COS(@NewDoubleValue));";

        protected override string ExceptedExpSql => "INSERT INTO destination_entities (\"double_value\") VALUES (EXP(@NewDoubleValue));";

        protected override string ExceptedFloorSql => "INSERT INTO destination_entities (\"double_value\") VALUES (FLOOR(@NewDoubleValue));";

    }
}
