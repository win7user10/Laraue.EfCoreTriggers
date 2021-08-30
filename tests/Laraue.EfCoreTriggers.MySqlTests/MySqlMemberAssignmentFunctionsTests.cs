using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    [UnitTest]
    public class MySqlMemberAssignmentFunctionsTests : BaseMemberAssignmentUnitTests
    {
        public MySqlMemberAssignmentFunctionsTests() : base(new MySqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedEnumValueSql => "INSERT INTO users (role) VALUES (999);";

        public override string ExceptedDecimalAddSql => "INSERT INTO test_entities (decimal_value) VALUES (NEW.decimal_value + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO test_entities (double_value) VALUES (NEW.double_value - 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO test_entities (int_value) VALUES (NEW.int_value * 2);";

        public override string ExceptedBooleanSql => "INSERT INTO test_entities (boolean_value) VALUES (true);";

        public override string ExceptedNewGuidSql => "INSERT INTO test_entities (guid_value) VALUES (UUID());";

    }
}
