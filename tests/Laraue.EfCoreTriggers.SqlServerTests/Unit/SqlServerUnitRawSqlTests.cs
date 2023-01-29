using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitRawSqlTests : UnitRawSqlTests
    {
        public SqlServerUnitRawSqlTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqlServerServices()))
        {
        }

        protected override string ExceptedInsertTriggerSqlForMemberArgs => "PERFORM func(@NewBooleanValue, @NewDoubleValue, \"source_entities\")";
        protected override string ExceptedInsertTriggerSqlForComputedArgs => "PERFORM func(@NewDoubleValue + 10)";
        protected override string ExceptedInsertTriggerSqlWhenNoArgs => "PERFORM func()";
        protected override string ExceptedUpdateTriggerSqlForMemberArgs => "PERFORM func(@OldDecimalValue, @NewDecimalValue)";
        protected override string ExceptedDeleteTriggerSqlForMemberArgs => "PERFORM func(@OldDecimalValue, @OldDoubleValue)";
    }
}