using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit;

public class MySqlUnitEnumerableFunctionsTests : UnitEnumerableFunctionsTests
{
    public MySqlUnitEnumerableFunctionsTests() : base(
        Helper.GetTriggerActionFactory(
            new ContextFactory().CreateDbContext().Model,
            collection => collection.AddMySqlServices()))
    {
    }

    protected override string ExceptedCountRelatedSql => "asdasd";

    protected override string ExceptedCountRelatedWithPredicateSql => "qweqwe";
}