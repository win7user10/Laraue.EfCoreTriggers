using Laraue.EfCoreTriggers.PostgreSql;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    [UnitTest]
    public class PostgreStringFunctionUnitTests : UnitStringFunctionsTests
    {
        public PostgreStringFunctionUnitTests() : base(new PostgreSqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedConcatSql => "INSERT INTO transactions_mirror (description) VALUES (CONCAT(NEW.description, 'abc'));";

        public override string ExceptedStringLowerSql => "INSERT INTO transactions_mirror (description) VALUES (LOWER(NEW.description));";

        public override string ExceptedStringUpperSql => "INSERT INTO transactions_mirror (description) VALUES (UPPER(NEW.description));";

        public override string ExceptedStringTrimSql => "INSERT INTO transactions_mirror (description) VALUES (BTRIM(NEW.description));";

        public override string ExceptedContainsSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (STRPOS(NEW.description, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (NEW.description LIKE ('%' || 'abc'));";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (NEW.description IS NULL OR NEW.description = '');";
    }
}
