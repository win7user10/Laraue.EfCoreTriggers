using Laraue.EfCoreTriggers.PostgreSql;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [UnitTest]
    public class PostgreSqlUnitStringFunctionTests : UnitStringFunctionsTests
    {
        public PostgreSqlUnitStringFunctionTests() : base(new PostgreSqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedConcatSql => "INSERT INTO destination_entities (string_field) VALUES (CONCAT(NEW.string_field, 'abc'));";

        public override string ExceptedStringLowerSql => "INSERT INTO destination_entities (string_field) VALUES (LOWER(NEW.string_field));";

        public override string ExceptedStringUpperSql => "INSERT INTO destination_entities (string_field) VALUES (UPPER(NEW.string_field));";

        public override string ExceptedStringTrimSql => "INSERT INTO destination_entities (string_field) VALUES (BTRIM(NEW.string_field));";

        public override string ExceptedContainsSql => "INSERT INTO destination_entities (boolean_value) VALUES (STRPOS(NEW.string_field, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO destination_entities (boolean_value) VALUES (NEW.string_field LIKE ('%' || 'abc'));";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO destination_entities (boolean_value) VALUES (NEW.string_field IS NULL OR NEW.string_field = '');";
    }
}
