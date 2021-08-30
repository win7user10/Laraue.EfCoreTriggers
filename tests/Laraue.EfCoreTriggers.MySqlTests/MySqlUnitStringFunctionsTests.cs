using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    [UnitTest]
    public class MySqlUnitStringFunctionsTests : UnitStringFunctionsTests
    {
        public MySqlUnitStringFunctionsTests() : base(new MySqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedConcatSql => "INSERT INTO transactions_mirror (description) VALUES (CONCAT(NEW.description, 'abc'));";

        public override string ExceptedStringLowerSql => "INSERT INTO transactions_mirror (description) VALUES (LOWER(NEW.description));";

        public override string ExceptedStringUpperSql => "INSERT INTO transactions_mirror (description) VALUES (UPPER(NEW.description));";

        public override string ExceptedStringTrimSql => "INSERT INTO transactions_mirror (description) VALUES (TRIM(NEW.description));";

        public override string ExceptedContainsSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (INSTR(NEW.description, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (NEW.description LIKE CONCAT('%', 'abc'));";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (NEW.description IS NULL OR NEW.description = '');";

    }
}
