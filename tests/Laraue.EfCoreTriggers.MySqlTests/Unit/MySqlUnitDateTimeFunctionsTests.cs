using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit;

public class MySqlUnitDateTimeFunctionsTests : UnitDateTimeFunctionsTests
{
    public MySqlUnitDateTimeFunctionsTests()
        : base(Helper.GetTriggerActionFactory(
            new ContextFactory().CreateDbContext().Model,
            collection => collection.AddMySqlServices()))
    {
    }

    protected override string ExceptedDateTimeUtcNowSql
        => "INSERT INTO `destination_entities` (`date_time_value`) SELECT UTC_TIMESTAMP();";
    
    protected override string ExceptedDateTimeNowSql
        => "INSERT INTO `destination_entities` (`date_time_value`) SELECT LOCALTIME();";
}