using Laraue.EfCoreTriggers.SqlLite;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Unit
{
    public class SqlLiteUnitRawSqlTests : UnitRawSqlTests
    {
        public SqlLiteUnitRawSqlTests() : base(new SqlLiteProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        protected override string ExceptedSqlForMemberArgs => "PERFORM func(NEW.boolean_value, NEW.double_value)";
        protected override string ExceptedSqlForComputedArgs => "PERFORM func(NEW.double_value + 10)";
        protected override string ExceptedSqlWhenNoArgs => "PERFORM func()";
    }
}