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

        protected override string ExceptedConcatSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT CONCAT(NEW.\"string_field\", 'abc');";

        protected override string ExceptedStringLowerSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT LOWER(NEW.\"string_field\");";

        protected override string ExceptedStringUpperSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT UPPER(NEW.\"string_field\");";

        protected override string ExceptedStringTrimSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT BTRIM(NEW.\"string_field\");";

        protected override string ExceptedContainsSql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT STRPOS(NEW.\"string_field\", 'abc') > 0;";

        protected override string ExceptedEndsWithSql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT NEW.\"string_field\" LIKE ('%' || 'abc');";

        protected override string ExceptedIsNullOrEmptySql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT NEW.\"string_field\" IS NULL OR NEW.\"string_field\" = '';";

        protected override string ExceptedCoalesceStringSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT COALESCE(NEW.\"string_field\", 'John');";
    }
}
