using Laraue.EfCoreTriggers.SqlLite;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Unit
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteUnitMemberAssignmentTests : BaseMemberAssignmentUnitTests
    {
        public SqlLiteUnitMemberAssignmentTests() : base(new SqlLiteProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        public override string ExceptedEnumValueSql => "INSERT INTO destination_entities (\"enum_value\") VALUES (NEW.enum_value);";

        public override string ExceptedDecimalAddSql => "INSERT INTO destination_entities (\"decimal_value\") VALUES (NEW.decimal_value + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO destination_entities (\"double_value\") VALUES (NEW.double_value + 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO destination_entities (\"int_value\") VALUES (NEW.int_value * 2);";

        public override string ExceptedBooleanSql => "INSERT INTO destination_entities (\"boolean_value\") VALUES (NEW.boolean_value is false);";

        public override string ExceptedNewGuidSql => "INSERT INTO destination_entities (\"guid_value\") VALUES (lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6))));";

    }
}
