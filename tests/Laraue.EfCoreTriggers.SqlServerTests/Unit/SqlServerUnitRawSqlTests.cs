using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    public class SqlServerUnitRawSqlTests : UnitRawSqlTests
    {
        public SqlServerUnitRawSqlTests() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        protected override string ExceptedSqlForMemberArgs => "PERFORM func(CASE WHEN @NewBooleanValue THEN 1 ELSE 0 END, @NewDoubleValue)";
        protected override string ExceptedSqlForComputedArgs => "PERFORM func(@NewDoubleValue + 10)";
        protected override string ExceptedSqlWhenNoArgs => "PERFORM func()";
    }
}