using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit
{
    public class MySqlUnitRawSqlTests : UnitRawSqlTests
    {
        public MySqlUnitRawSqlTests() : base(new MySqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        protected override string ExceptedSqlForMemberArgs => "PERFORM func(NEW.boolean_value, NEW.double_value)";
        protected override string ExceptedSqlForComputedArgs => "PERFORM func(NEW.double_value + 10)";
        protected override string ExceptedSqlWhenNoArgs => "PERFORM func()";
    }
}