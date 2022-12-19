using Laraue.EfCoreTriggers.SqlLite.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Unit
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteUnitMemberAssignmentTests : BaseMemberAssignmentUnitTests
    {
        public SqlLiteUnitMemberAssignmentTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqliteServices()))
        {
        }

        public override string ExceptedEnumValueSql => "INSERT INTO \"destination_entities\" (\"enum_value\") SELECT NEW.\"enum_value\";";

        public override string ExceptedDecimalAddSql => "INSERT INTO \"destination_entities\" (\"decimal_value\") SELECT NEW.\"decimal_value\" + 3;";

        public override string ExceptedDoubleSubSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT NEW.\"double_value\" + 3;";

        public override string ExceptedIntMultiplySql => "INSERT INTO \"destination_entities\" (\"int_value\") SELECT NEW.\"int_value\" * 2;";

        public override string ExceptedBooleanSql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT NEW.\"boolean_value\" IS FALSE;";

        public override string ExceptedNewGuidSql => "INSERT INTO \"destination_entities\" (\"guid_value\") SELECT lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)));";

        public override string ExceptedCharVariableSql => "INSERT INTO \"destination_entities\" (\"char_value\") SELECT NEW.\"char_value\";";

        public override string ExceptedCharValueSql => "INSERT INTO \"destination_entities\" (\"char_value\") SELECT 'a';";
        
        public override string ExceptedNewDateTimeOffsetSql => "INSERT INTO \"destination_entities\" (\"date_time_offset_value\") SELECT DATE(\"now\");";
    }
}
