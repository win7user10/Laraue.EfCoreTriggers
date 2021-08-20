using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    public class MySqlGeneratingExpressionsTests : BaseGeneratingExpressionsTests
    {
        public MySqlGeneratingExpressionsTests() : base(new MySqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedConcatSql => "INSERT INTO transactions_mirror (description) VALUES (CONCAT(NEW.description, 'abc'));";

        public override string ExceptedStringLowerSql => "INSERT INTO transactions_mirror (description) VALUES (LOWER(NEW.description));";

        public override string ExceptedStringUpperSql => "INSERT INTO transactions_mirror (description) VALUES (UPPER(NEW.description));";

        public override string ExceptedEnumValueSql => "INSERT INTO users (role) VALUES (999);";

        public override string ExceptedDecimalAddSql => "INSERT INTO test_entities (decimal_value) VALUES (NEW.decimal_value + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO test_entities (double_value) VALUES (NEW.double_value - 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO test_entities (int_value) VALUES (NEW.int_value * 2);";

		public override string ExceptedBooleanSql => "INSERT INTO test_entities (boolean_value) VALUES (true);";

        public override string ExceptedNewGuidSql => "INSERT INTO test_entities (guid_value) VALUES (UUID());";

        public override string ExceptedStringTrimSql => "INSERT INTO transactions_mirror (description) VALUES (TRIM(NEW.description));";

        public override string ExceptedContainsSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (INSTR(NEW.description, 'abc') > 0);";
       
        public override string ExceptedEndsWithSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (NEW.description LIKE CONCAT('%', 'abc'));";

    }
}
