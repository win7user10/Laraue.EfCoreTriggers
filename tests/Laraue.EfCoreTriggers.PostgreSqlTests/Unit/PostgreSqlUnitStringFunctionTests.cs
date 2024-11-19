using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlUnitStringFunctionTests : UnitStringFunctionsTests
    {
        public PostgreSqlUnitStringFunctionTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddPostgreSqlServices()))
        {
        }

        protected override string ExceptedConcatSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT CONCAT(NEW.\"StringField\", 'abc');";

        protected override string ExceptedStringLowerSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT LOWER(NEW.\"StringField\");";

        protected override string ExceptedStringUpperSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT UPPER(NEW.\"StringField\");";

        protected override string ExceptedStringTrimSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT BTRIM(NEW.\"StringField\");";

        protected override string ExceptedContainsSql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT STRPOS(NEW.\"StringField\", 'abc') > 0;";

        protected override string ExceptedEndsWithSql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT NEW.\"StringField\" LIKE ('%' || 'abc');";

        protected override string ExceptedIsNullOrEmptySql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT NEW.\"StringField\" IS NULL OR NEW.\"StringField\" = '';";

        protected override string ExceptedCoalesceStringSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT COALESCE(NEW.\"StringField\", 'John');";
    }
}
