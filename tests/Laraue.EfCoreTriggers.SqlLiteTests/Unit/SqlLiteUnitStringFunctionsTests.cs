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

        public override string ExceptedConcatSql => "INSERT INTO destination_entities (\"string_field\") SELECT NEW.string_field || 'abc';";

        public override string ExceptedStringLowerSql => "INSERT INTO destination_entities (\"string_field\") SELECT LOWER(NEW.string_field);";

        public override string ExceptedStringUpperSql => "INSERT INTO destination_entities (\"string_field\") SELECT UPPER(NEW.string_field);";

        public override string ExceptedStringTrimSql => "INSERT INTO destination_entities (\"string_field\") SELECT TRIM(NEW.string_field);";

        public override string ExceptedContainsSql => "INSERT INTO destination_entities (\"boolean_value\") SELECT INSTR(NEW.string_field, 'abc') > 0;";

        public override string ExceptedEndsWithSql => "INSERT INTO destination_entities (\"boolean_value\") SELECT NEW.string_field LIKE ('%' || 'abc');";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO destination_entities (\"boolean_value\") SELECT NEW.string_field IS NULL OR NEW.string_field = '';";
    }
}
