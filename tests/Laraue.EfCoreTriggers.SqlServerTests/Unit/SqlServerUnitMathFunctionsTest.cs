using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit 
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitMathFunctionsTest : UnitMathFunctionsTests
    {
        public SqlServerUnitMathFunctionsTest() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedAbsSql => "INSERT INTO destination_entities (decimal_value) VALUES (ABS(@NewDecimalValue));";

        public override string ExceptedAcosSql => "INSERT INTO destination_entities (double_value) VALUES (ACOS(@NewDoubleValue));";

        public override string ExceptedAsinSql => "INSERT INTO destination_entities (double_value) VALUES (ASIN(@NewDoubleValue));";

        public override string ExceptedAtanSql => "INSERT INTO destination_entities (double_value) VALUES (ATAN(@NewDoubleValue));";

        public override string ExceptedAtan2Sql => "INSERT INTO destination_entities (double_value) VALUES (ATN2(@NewDoubleValue, @NewDoubleValue));";

        public override string ExceptedCeilingSql => "INSERT INTO destination_entities (double_value) VALUES (CEILING(@NewDoubleValue));";

        public override string ExceptedCosSql => "INSERT INTO destination_entities (double_value) VALUES (COS(@NewDoubleValue));";

        public override string ExceptedExpSql => "INSERT INTO destination_entities (double_value) VALUES (EXP(@NewDoubleValue));";

        public override string ExceptedFloorSql => "INSERT INTO destination_entities (double_value) VALUES (FLOOR(@NewDoubleValue));";

    }
}
