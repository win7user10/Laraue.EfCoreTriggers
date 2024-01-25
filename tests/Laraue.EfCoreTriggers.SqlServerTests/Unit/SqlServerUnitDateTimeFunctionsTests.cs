using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit;

public class SqlServerUnitDateTimeFunctionsTests : UnitDateTimeFunctionsTests
{
    public SqlServerUnitDateTimeFunctionsTests()
        : base(Helper.GetTriggerActionFactory(
            new ContextFactory().CreateDbContext().Model,
            collection => collection.AddSqlServerServices()))
    {
    }

    protected override string ExceptedDateTimeUtcNowSql
        => "INSERT INTO \"destination_entities\" (\"date_time_value\") SELECT GETUTCDATE();";
    
    protected override string ExceptedDateTimeNowSql
        => "INSERT INTO \"destination_entities\" (\"date_time_value\") SELECT GETDATE();";
}