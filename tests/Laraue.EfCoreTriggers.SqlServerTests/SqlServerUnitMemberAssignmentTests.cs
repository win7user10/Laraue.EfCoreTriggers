using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    [UnitTest]
    public class SqlServerUnitMemberAssignmentTests : BaseMemberAssignmentUnitTests
    {
        public SqlServerUnitMemberAssignmentTests() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedEnumValueSql => "INSERT INTO users (role) VALUES (999);";

        public override string ExceptedDecimalAddSql => "INSERT INTO test_entities (decimal_value) VALUES (@NewDecimalValue + 3);";

        public override string ExceptedDoubleSubSql => "INSERT INTO test_entities (double_value) VALUES (@NewDoubleValue - 3);";

        public override string ExceptedIntMultiplySql => "INSERT INTO test_entities (int_value) VALUES (@NewIntValue * 2);";

        public override string ExceptedBooleanSql => "INSERT INTO test_entities (boolean_value) VALUES (1);";

        public override string ExceptedNewGuidSql => "INSERT INTO test_entities (guid_value) VALUES (NEWID());";

       
        
    }
}