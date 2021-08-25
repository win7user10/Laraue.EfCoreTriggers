using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests.Tests;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    [UnitTest]
    public class SqlServerGeneratingExpressionsTests : BaseGeneratingExpressionsTests
    {
        public SqlServerGeneratingExpressionsTests() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedConcatSql => "INSERT INTO transactions_mirror (description) VALUES (@NewDescription + 'abc');";

        public override string ExceptedStringLowerSql => "INSERT INTO transactions_mirror (description) VALUES (LOWER(@NewDescription));";

        public override string ExceptedStringUpperSql => "INSERT INTO transactions_mirror (description) VALUES (UPPER(@NewDescription));";

        public override string ExceptedEnumValueSql => "INSERT INTO users (role) VALUES (999);";

        public override string ExceptedDecimalAddSql => "INSERT INTO test_entities (decimal_value) VALUES (@NewDecimalValue + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO test_entities (double_value) VALUES (@NewDoubleValue - 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO test_entities (int_value) VALUES (@NewIntValue * 2);";

        public override string ExceptedBooleanSql => "INSERT INTO test_entities (boolean_value) VALUES (1);";

        public override string ExceptedNewGuidSql => "INSERT INTO test_entities (guid_value) VALUES (NEWID());";

        public override string ExceptedStringTrimSql => "INSERT INTO transactions_mirror (description) VALUES (TRIM(@NewDescription));";

        public override string ExceptedContainsSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (CHARINDEX(@NewDescription, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (@NewDescription LIKE ('%' + 'abc'));";
        
        public override string ExceptedIsNullOrEmptySql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (@NewDescription IS NULL OR @NewDescription = '');";
        
        public override string ExceptedAbsSql => "INSERT INTO transactions_mirror (value) VALUES (ABS(@NewValue));";

    }
}