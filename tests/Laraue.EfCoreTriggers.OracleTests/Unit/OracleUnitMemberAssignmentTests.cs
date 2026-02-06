using Laraue.EfCoreTriggers.Oracle.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.OracleTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class OracleUnitMemberAssignmentTests : BaseMemberAssignmentUnitTests
    {
        public OracleUnitMemberAssignmentTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddOracleServices()))
        {
        }
        
        public override string ExceptedEnumValueSql
            => "INSERT INTO \"DestinationEntities\" (\"EnumValue\") SELECT :NEW.\"EnumValue\";";

        public override string ExceptedDecimalAddSql
            => "INSERT INTO \"DestinationEntities\" (\"DecimalValue\") SELECT :NEW.\"DecimalValue\" + 3;";

        public override string ExceptedDoubleSubSql
            => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT :NEW.\"DoubleValue\" - 1;";

        public override string ExceptedIntMultiplySql
            => "INSERT INTO \"DestinationEntities\" (\"IntValue\") SELECT :NEW.\"IntValue\" * 2;";

        public override string ExceptedBooleanSql
            => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT CASE WHEN :NEW.\"BooleanValue\" IS FALSE THEN 1 ELSE 0 END;";

        public override string ExceptedNewGuidSql
            => "INSERT INTO \"DestinationEntities\" (\"GuidValue\") SELECT SYS_GUID();";

        public override string ExceptedCharVariableSql
            => "INSERT INTO \"DestinationEntities\" (\"CharValue\") SELECT :NEW.\"CharValue\";";

        public override string ExceptedCharValueSql
            => "INSERT INTO \"DestinationEntities\" (\"CharValue\") SELECT 'a';";
        
        public override string ExceptedDateTimeOffsetNowSql
            => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT CURRENT_DATE;";
        
        public override string ExceptedDateTimeOffsetUtcNowSql
            => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT SYS_EXTRACT_UTC(SYSTIMESTAMP);";

        public override string ExceptedNewDateTimeOffsetSql
            => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT TO_DATE('1000-01-01', 'YYYY-MM-DD');";

        public override string ExceptedNewDateTimeSql
            => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT TO_DATE('1000-01-01', 'YYYY-MM-DD');";
    }
}