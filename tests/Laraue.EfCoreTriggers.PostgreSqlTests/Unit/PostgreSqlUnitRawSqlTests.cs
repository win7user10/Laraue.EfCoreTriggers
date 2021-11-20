using Laraue.EfCoreTriggers.PostgreSql;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    public class PostgreSqlUnitRawSqlTests : UnitRawSqlTests
    {
        public PostgreSqlUnitRawSqlTests() : base(new PostgreSqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        protected override string ExceptedSqlForMemberArgs => "PERFORM func(NEW.boolean_value, NEW.double_value)";
        protected override string ExceptedSqlForComputedArgs => "PERFORM func(NEW.double_value + 10)";
        protected override string ExceptedSqlWhenNoArgs => "PERFORM func()";
    }
}