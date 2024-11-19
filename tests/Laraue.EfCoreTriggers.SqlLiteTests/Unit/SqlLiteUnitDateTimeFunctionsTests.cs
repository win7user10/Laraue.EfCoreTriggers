using Laraue.EfCoreTriggers.SqlLite.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Unit;

public class SqlLiteUnitDateTimeFunctionsTests : UnitDateTimeFunctionsTests
{
    public SqlLiteUnitDateTimeFunctionsTests()
        : base(Helper.GetTriggerActionFactory(
            new ContextFactory().CreateDbContext().Model,
            collection => collection.AddSqliteServices()))
    {
    }

    protected override string ExceptedDateTimeUtcNowSql
        => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT DATETIME('now');";
    
    protected override string ExceptedDateTimeNowSql
        => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT DATETIME('now', 'localtime');";
}