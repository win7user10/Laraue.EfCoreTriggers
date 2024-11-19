using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit;

public class PostgreSqlUnitDateTimeFunctionsTests : UnitDateTimeFunctionsTests
{
    public PostgreSqlUnitDateTimeFunctionsTests()
        : base(Helper.GetTriggerActionFactory(
            new ContextFactory().CreateDbContext().Model,
            collection => collection.AddPostgreSqlServices()))
    {
    }

    protected override string ExceptedDateTimeUtcNowSql
        => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT CURRENT_TIMESTAMP;";
    
    protected override string ExceptedDateTimeNowSql
        => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT NOW();";
}