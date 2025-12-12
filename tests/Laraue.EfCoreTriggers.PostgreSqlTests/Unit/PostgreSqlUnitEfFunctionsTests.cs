using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlUnitEfFunctionsTests : UnitEfFunctionsTests
    {
        public PostgreSqlUnitEfFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddPostgreSqlServices()))
        {
        }

        protected override string ExceptedEfPropertyTranslationSql => "INSERT INTO \"DestinationEntities\" (\"IntValue\") SELECT NEW.\"IntValue\";";
    }
}
