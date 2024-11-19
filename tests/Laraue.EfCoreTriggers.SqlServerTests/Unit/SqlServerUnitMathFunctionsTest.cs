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

        protected override string ExceptedAbsSql => "INSERT INTO \"DestinationEntities\" (\"DecimalValue\") SELECT ABS(@NewDecimalValue);";

        protected override string ExceptedAcosSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ACOS(@NewDoubleValue);";

        protected override string ExceptedAsinSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ASIN(@NewDoubleValue);";

        protected override string ExceptedAtanSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ATAN(@NewDoubleValue);";

        protected override string ExceptedAtan2Sql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ATN2(@NewDoubleValue, @NewDoubleValue);";

        protected override string ExceptedCeilingSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT CEILING(@NewDoubleValue);";

        protected override string ExceptedCosSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT COS(@NewDoubleValue);";

        protected override string ExceptedExpSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT EXP(@NewDoubleValue);";

        protected override string ExceptedFloorSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT FLOOR(@NewDoubleValue);";
    }
}
