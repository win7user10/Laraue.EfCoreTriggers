using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit
{
    [Collection(CollectionNames.MySql)]
    public class MySqlUnitEfFunctionsTests : UnitEfFunctionsTests
    {
        public MySqlUnitEfFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddMySqlServices()))
        {
        }

        protected override string ExceptedEfPropertyTranslationSql => "INSERT INTO `DestinationEntities` (`IntValue`) SELECT NEW.`IntValue`;";
    }
}
