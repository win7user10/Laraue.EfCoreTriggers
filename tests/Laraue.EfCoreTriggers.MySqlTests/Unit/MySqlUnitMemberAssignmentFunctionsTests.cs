using Laraue.EfCoreTriggers.MySql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit
{
    [Collection(CollectionNames.MySql)]
    public class MySqlUnitMemberAssignmentFunctionsTests : BaseMemberAssignmentUnitTests
    {
        public MySqlUnitMemberAssignmentFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddMySqlServices()))
        {
        }

        public override string ExceptedEnumValueSql => "INSERT INTO `DestinationEntities` (`EnumValue`) SELECT NEW.`EnumValue`;";

        public override string ExceptedDecimalAddSql => "INSERT INTO `DestinationEntities` (`DecimalValue`) SELECT NEW.`DecimalValue` + 3;";

        public override string ExceptedDoubleSubSql => "INSERT INTO `DestinationEntities` (`DoubleValue`) SELECT NEW.`DoubleValue` + 3;";

        public override string ExceptedIntMultiplySql => "INSERT INTO `DestinationEntities` (`IntValue`) SELECT NEW.`IntValue` * 2;";

        public override string ExceptedBooleanSql => "INSERT INTO `DestinationEntities` (`BooleanValue`) SELECT NEW.`BooleanValue` IS FALSE;";

        public override string ExceptedNewGuidSql => "INSERT INTO `DestinationEntities` (`GuidValue`) SELECT UUID();";
        
        public override string ExceptedCharVariableSql => "INSERT INTO `DestinationEntities` (`CharValue`) SELECT NEW.`CharValue`;";
        
        public override string ExceptedCharValueSql => "INSERT INTO `DestinationEntities` (`CharValue`) SELECT 'a';";
        
        public override string ExceptedDateTimeOffsetNowSql => "INSERT INTO `DestinationEntities` (`DateTimeOffsetValue`) SELECT LOCALTIME();";
        public override string ExceptedDateTimeOffsetUtcNowSql => "INSERT INTO `DestinationEntities` (`DateTimeOffsetValue`) SELECT UTC_TIMESTAMP();";

        public override string ExceptedNewDateTimeOffsetSql => "INSERT INTO `DestinationEntities` (`DateTimeOffsetValue`) SELECT '1000-01-01';";
        public override string ExceptedNewDateTimeSql => "INSERT INTO `DestinationEntities` (`DateTimeValue`) SELECT '1000-01-01';";
    }
}
