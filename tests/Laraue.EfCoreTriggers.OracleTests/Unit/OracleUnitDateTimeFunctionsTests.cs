using Laraue.EfCoreTriggers.Oracle.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.OracleTests.Unit;

public class OracleUnitDateTimeFunctionsTests : UnitDateTimeFunctionsTests
{
    public OracleUnitDateTimeFunctionsTests()
        : base(Helper.GetTriggerActionFactory(
            new ContextFactory().CreateDbContext().Model,
            collection => collection.AddOracleServices()))
    {
    }

    protected override string ExceptedDateTimeUtcNowSql
        => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT SYS_EXTRACT_UTC(SYSTIMESTAMP);";
    
    protected override string ExceptedDateTimeNowSql
        => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT CURRENT_DATE;";
}