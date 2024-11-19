using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlUnitMathFunctionsTests : UnitMathFunctionsTests
    {
        public PostgreSqlUnitMathFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddPostgreSqlServices()))
        {
        }

        protected override string ExceptedAbsSql => "INSERT INTO \"DestinationEntities\" (\"DecimalValue\") SELECT ABS(NEW.\"DecimalValue\");";

        protected override string ExceptedAcosSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ACOS(NEW.\"DoubleValue\");";

        protected override string ExceptedAsinSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ASIN(NEW.\"DoubleValue\");";

        protected override string ExceptedAtanSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ATAN(NEW.\"DoubleValue\");";

        protected override string ExceptedAtan2Sql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT ATAN2(NEW.\"DoubleValue\", NEW.\"DoubleValue\");";

        protected override string ExceptedCeilingSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT CEILING(NEW.\"DoubleValue\");";

        protected override string ExceptedCosSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT COS(NEW.\"DoubleValue\");";

        protected override string ExceptedExpSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT EXP(NEW.\"DoubleValue\");";

        protected override string ExceptedFloorSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT FLOOR(NEW.\"DoubleValue\");"; 
    }
}
