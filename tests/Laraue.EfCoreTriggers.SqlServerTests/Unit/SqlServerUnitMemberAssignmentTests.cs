using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    [UnitTest]
    public class SqlServerUnitMemberAssignmentTests : BaseMemberAssignmentUnitTests
    {
        public SqlServerUnitMemberAssignmentTests() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedEnumValueSql => "INSERT INTO destination_entities (enum_value) VALUES (@NewEnumValue);";

        public override string ExceptedDecimalAddSql => "INSERT INTO destination_entities (decimal_value) VALUES (@NewDecimalValue + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO destination_entities (double_value) VALUES (@NewDoubleValue + 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO destination_entities (int_value) VALUES (@NewIntValue * 2);";

        public override string ExceptedBooleanSql => "INSERT INTO destination_entities (boolean_value) VALUES (CASE WHEN @NewBooleanValue = 0 THEN 1 ELSE 0 END);";

        public override string ExceptedNewGuidSql => "INSERT INTO destination_entities (guid_value) VALUES (NEWID());";

       
        
    }
}