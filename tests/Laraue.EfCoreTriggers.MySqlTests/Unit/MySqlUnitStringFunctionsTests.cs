using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit
{
    [Collection(CollectionNames.MySql)]
    public class MySqlUnitStringFunctionsTests : UnitStringFunctionsTests
    {
        public MySqlUnitStringFunctionsTests() : base(new MySqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedConcatSql => "INSERT INTO destination_entities (`string_field`) VALUES (CONCAT(NEW.string_field, 'abc'));";

        public override string ExceptedStringLowerSql => "INSERT INTO destination_entities (`string_field`) VALUES (LOWER(NEW.string_field));";

        public override string ExceptedStringUpperSql => "INSERT INTO destination_entities (`string_field`) VALUES (UPPER(NEW.string_field));";

        public override string ExceptedStringTrimSql => "INSERT INTO destination_entities (`string_field`) VALUES (TRIM(NEW.string_field));";

        public override string ExceptedContainsSql => "INSERT INTO destination_entities (`boolean_value`) VALUES (INSTR(NEW.string_field, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO destination_entities (`boolean_value`) VALUES (NEW.string_field LIKE CONCAT('%', 'abc'));";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO destination_entities (`boolean_value`) VALUES (NEW.string_field IS NULL OR NEW.string_field = '');";

    }
}
