using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using Laraue.EfCoreTriggers.Tests;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    public class SqlServerGeneratingExpressionsTests : BaseGeneratingExpressionsTests
    {
        public SqlServerGeneratingExpressionsTests() : base(new SqlServerSqlVisitor(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedConcatSql => "INSERT INTO transactions_mirror (description) VALUES (@NewDescription + 'abc');";

        public override string ExceptedStringLowerSql => "INSERT INTO transactions_mirror (description) VALUES (LOWER(@NewDescription));";

        public override string ExceptedStringUpperSql => "INSERT INTO transactions_mirror (description) VALUES (UPPER(@NewDescription));";

        public override string ExceptedEnumValueSql => "INSERT INTO users (role) VALUES (999);";

        public override string ExceptedDecimalAddSql => "INSERT INTO test_entities (decimal_value) VALUES (@NewDecimalValue + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO test_entities (double_value) VALUES (@NewDoubleValue - 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO test_entities (int_value) VALUES (@NewIntValue * 2);";
    }
}