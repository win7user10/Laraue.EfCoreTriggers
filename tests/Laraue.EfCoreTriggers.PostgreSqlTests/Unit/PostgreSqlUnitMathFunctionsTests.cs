using Laraue.EfCoreTriggers.PostgreSql;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlUnitMathFunctionsTests : UnitMathFunctionsTests
    {
        public PostgreSqlUnitMathFunctionsTests() : base(new PostgreSqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        protected override string ExceptedAbsSql => "INSERT INTO destination_entities (\"decimal_value\") VALUES (ABS(NEW.decimal_value));";

        protected override string ExceptedAcosSql => "INSERT INTO destination_entities (\"double_value\") VALUES (ACOS(NEW.double_value));";

        protected override string ExceptedAsinSql => "INSERT INTO destination_entities (\"double_value\") VALUES (ASIN(NEW.double_value));";

        protected override string ExceptedAtanSql => "INSERT INTO destination_entities (\"double_value\") VALUES (ATAN(NEW.double_value));";

        protected override string ExceptedAtan2Sql => "INSERT INTO destination_entities (\"double_value\") VALUES (ATAN2(NEW.double_value, NEW.double_value));";

        protected override string ExceptedCeilingSql => "INSERT INTO destination_entities (\"double_value\") VALUES (CEILING(NEW.double_value));";

        protected override string ExceptedCosSql => "INSERT INTO destination_entities (\"double_value\") VALUES (COS(NEW.double_value));";

        protected override string ExceptedExpSql => "INSERT INTO destination_entities (\"double_value\") VALUES (EXP(NEW.double_value));";

        protected override string ExceptedFloorSql => "INSERT INTO destination_entities (\"double_value\") VALUES (FLOOR(NEW.double_value));"; 
    }
}
