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
        
        public override string ExceptedEnumValueSql => "INSERT INTO \"destination_entities\" (\"enum_value\") SELECT @NewEnumValue;";

        public override string ExceptedDecimalAddSql => "INSERT INTO \"destination_entities\" (\"decimal_value\") SELECT @NewDecimalValue + 3;";

        public override string ExceptedDoubleSubSql => "INSERT INTO \"destination_entities\" (\"double_value\") SELECT @NewDoubleValue + 3;";

        public override string ExceptedIntMultiplySql => "INSERT INTO \"destination_entities\" (\"int_value\") SELECT @NewIntValue * 2;";

        public override string ExceptedBooleanSql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT CASE WHEN @NewBooleanValue = 0 THEN 1 ELSE 0 END;";

        public override string ExceptedNewGuidSql => "INSERT INTO \"destination_entities\" (\"guid_value\") SELECT NEWID();";

        public override string ExceptedCharVariableSql => "INSERT INTO \"destination_entities\" (\"char_value\") SELECT @NewCharValue;";

        public override string ExceptedCharValueSql => "INSERT INTO \"destination_entities\" (\"char_value\") SELECT 'a';";
        
        public override string ExceptedNewDateTimeOffsetSql => "INSERT INTO \"destination_entities\" (\"date_time_offset_value\") SELECT GETDATE();";
    }
}