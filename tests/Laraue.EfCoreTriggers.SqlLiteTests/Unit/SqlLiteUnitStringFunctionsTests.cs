using Laraue.EfCoreTriggers.SqlLite.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Unit
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteUnitStringFunctionsTests : UnitStringFunctionsTests
    {
        public SqlLiteUnitStringFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqliteServices()))
        {
        }

        protected override string ExceptedConcatSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT NEW.\"StringField\" || 'abc';";

        protected override string ExceptedStringLowerSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT LOWER(NEW.\"StringField\");";

        protected override string ExceptedStringUpperSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT UPPER(NEW.\"StringField\");";

        protected override string ExceptedStringTrimSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT TRIM(NEW.\"StringField\");";

        protected override string ExceptedContainsSql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT INSTR(NEW.\"StringField\", 'abc') > 0;";

        protected override string ExceptedEndsWithSql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT NEW.\"StringField\" LIKE ('%' || 'abc');";

        protected override string ExceptedIsNullOrEmptySql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT NEW.\"StringField\" IS NULL OR NEW.\"StringField\" = '';";

        protected override string ExceptedCoalesceStringSql => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT COALESCE(NEW.\"StringField\", 'John');";
    }
}
