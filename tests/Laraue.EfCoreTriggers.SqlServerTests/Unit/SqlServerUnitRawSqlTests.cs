using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitRawSqlTests : UnitRawSqlTests
    {
        public SqlServerUnitRawSqlTests() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }

        protected override string ExceptedInsertTriggerSqlForMemberArgs => "PERFORM func(CASE WHEN @NewBooleanValue THEN 1 ELSE 0 END, @NewDoubleValue)";
        protected override string ExceptedInsertTriggerSqlForComputedArgs => "PERFORM func(@NewDoubleValue + 10)";
        protected override string ExceptedInsertTriggerSqlWhenNoArgs => "PERFORM func()";
        protected override string ExceptedUpdateTriggerSqlForMemberArgs => "PERFORM func(@OldDecimalValue, @NewDecimalValue)";
        protected override string ExceptedDeleteTriggerSqlForMemberArgs => "PERFORM func(@OldDecimalValue, @OldDoubleValue)";
    }
}