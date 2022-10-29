using Laraue.EfCoreTriggers.SqlLite.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Unit
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteUnitMathFunctionsTests : UnitMathFunctionsTests
    {
        public SqlLiteUnitMathFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqliteServices()))
        {
        }

        protected override string ExceptedAbsSql => "INSERT INTO \"destination_entities\" (\"decimal_value\") SELECT ABS(NEW.\"decimal_value\");";

        protected override string ExceptedAcosSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ACOS(NEW.\"double_value\");";

        protected override string ExceptedAsinSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ASIN(NEW.\"double_value\");";

        protected override string ExceptedAtanSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ATAN(NEW.\"double_value\");";

        protected override string ExceptedAtan2Sql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ATAN2(NEW.\"double_value\", NEW.\"double_value\");";

        protected override string ExceptedCeilingSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT CEIL(NEW.\"double_value\");";

        protected override string ExceptedCosSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT COS(NEW.\"double_value\");";

        protected override string ExceptedExpSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT EXP(NEW.\"double_value\");";

        protected override string ExceptedFloorSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT FLOOR(NEW.\"double_value\");";
    }
}
