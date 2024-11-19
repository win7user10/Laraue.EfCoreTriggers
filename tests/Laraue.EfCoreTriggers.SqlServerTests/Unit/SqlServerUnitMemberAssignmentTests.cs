using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitMemberAssignmentTests : BaseMemberAssignmentUnitTests
    {
        public SqlServerUnitMemberAssignmentTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqlServerServices()))
        {
        }
        
        public override string ExceptedEnumValueSql => "INSERT INTO \"DestinationEntities\" (\"EnumValue\") SELECT @NewEnumValue;";

        public override string ExceptedDecimalAddSql => "INSERT INTO \"DestinationEntities\" (\"DecimalValue\") SELECT @NewDecimalValue + 3;";

        public override string ExceptedDoubleSubSql => "INSERT INTO \"DestinationEntities\" (\"DoubleValue\") SELECT @NewDoubleValue + 3;";

        public override string ExceptedIntMultiplySql => "INSERT INTO \"DestinationEntities\" (\"IntValue\") SELECT @NewIntValue * 2;";

        public override string ExceptedBooleanSql => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT CASE WHEN @NewBooleanValue = 0 THEN 1 ELSE 0 END;";

        public override string ExceptedNewGuidSql => "INSERT INTO \"DestinationEntities\" (\"GuidValue\") SELECT NEWID();";

        public override string ExceptedCharVariableSql => "INSERT INTO \"DestinationEntities\" (\"CharValue\") SELECT @NewCharValue;";

        public override string ExceptedCharValueSql => "INSERT INTO \"DestinationEntities\" (\"CharValue\") SELECT 'a';";
        public override string ExceptedDateTimeOffsetNowSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT SYSDATETIME();";
        public override string ExceptedDateTimeOffsetUtcNowSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT SYSUTCDATETIME();";

        public override string ExceptedNewDateTimeOffsetSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeOffsetValue\") SELECT '1753-01-01';";

        public override string ExceptedNewDateTimeSql => "INSERT INTO \"DestinationEntities\" (\"DateTimeValue\") SELECT '1753-01-01';";
    }
}