using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit
{
    [Collection(CollectionNames.MySql)]
    public class MySqlUnitMemberAssignmentFunctionsTests : BaseMemberAssignmentUnitTests
    {
        public MySqlUnitMemberAssignmentFunctionsTests() : base(new MySqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedEnumValueSql => "INSERT INTO destination_entities (enum_value) VALUES (NEW.enum_value);";

        public override string ExceptedDecimalAddSql => "INSERT INTO destination_entities (decimal_value) VALUES (NEW.decimal_value + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO destination_entities (double_value) VALUES (NEW.double_value + 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO destination_entities (int_value) VALUES (NEW.int_value * 2);";

        public override string ExceptedBooleanSql => "INSERT INTO destination_entities (boolean_value) VALUES (NEW.boolean_value is false);";

        public override string ExceptedNewGuidSql => "INSERT INTO destination_entities (guid_value) VALUES (UUID());";

    }
}
