using Laraue.EfCoreTriggers.Oracle.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.OracleTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class OracleUnitEfFunctionsTests : UnitEfFunctionsTests
    {
        public OracleUnitEfFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddOracleServices()))
        {
        }

        protected override string ExceptedEfPropertyTranslationSql
            => "INSERT INTO \"DestinationEntities\" (\"IntValue\") SELECT :NEW.\"IntValue\";";
    }
}
