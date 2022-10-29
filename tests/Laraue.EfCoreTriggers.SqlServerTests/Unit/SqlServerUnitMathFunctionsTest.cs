using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit 
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitMathFunctionsTest : UnitMathFunctionsTests
    {
        public SqlServerUnitMathFunctionsTest() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqlServerServices()))
        {
        }

        protected override string ExceptedAbsSql => "INSERT INTO \"destination_entities\" (\"decimal_value\") SELECT ABS(@NewDecimalValue);";

        protected override string ExceptedAcosSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ACOS(@NewDoubleValue);";

        protected override string ExceptedAsinSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ASIN(@NewDoubleValue);";

        protected override string ExceptedAtanSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ATAN(@NewDoubleValue);";

        protected override string ExceptedAtan2Sql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT ATN2(@NewDoubleValue, @NewDoubleValue);";

        protected override string ExceptedCeilingSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT CEILING(@NewDoubleValue);";

        protected override string ExceptedCosSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT COS(@NewDoubleValue);";

        protected override string ExceptedExpSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT EXP(@NewDoubleValue);";

        protected override string ExceptedFloorSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT FLOOR(@NewDoubleValue);";
    }
}
