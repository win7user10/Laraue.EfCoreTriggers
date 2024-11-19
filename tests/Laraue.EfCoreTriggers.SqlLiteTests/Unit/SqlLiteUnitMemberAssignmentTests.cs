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

        public override string ExceptedEnumValueSql => "INSERT INTO \"DestinationEntities\" (\"EnumValue\") SELECT NEW.\"EnumValue\";";

        public override string ExceptedDecimalAddSql => "INSERT INTO \"DestinationEntities\" (\"DecimalValue\") SELECT NEW.\"DecimalValue\" + 3;";

        public override string ExceptedDoubleSubSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT NEW.\"DoubleValue\" + 3;";

        public override string ExceptedIntMultiplySql => "INSERT INTO \"DestinationEntities\" (\"IntValue\") SELECT NEW.\"IntValue\" * 2;";

        public override string ExceptedBooleanSql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT NEW.\"BooleanValue\" IS FALSE;";

        public override string ExceptedNewGuidSql => "INSERT INTO \"DestinationEntities\" (\"GuidValue\") SELECT lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)));";

        public override string ExceptedCharVariableSql => "INSERT INTO \"DestinationEntities\" (\"CharValue\") SELECT NEW.\"CharValue\";";

        public override string ExceptedCharValueSql => "INSERT INTO \"DestinationEntities\" (\"CharValue\") SELECT 'a';";
        public override string ExceptedDateTimeOffsetNowSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT DATETIME('now', 'localtime');";
        public override string ExceptedDateTimeOffsetUtcNowSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT DATETIME('now');";

        public override string ExceptedNewDateTimeOffsetSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT '0001-01-01T00:00:00+00:00';";
        public override string ExceptedNewDateTimeSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT '0001-01-01';";
    }
}
