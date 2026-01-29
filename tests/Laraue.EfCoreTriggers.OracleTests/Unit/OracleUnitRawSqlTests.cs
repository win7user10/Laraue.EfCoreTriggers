using Laraue.EfCoreTriggers.Oracle.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.OracleTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class OracleUnitRawSqlTests : UnitRawSqlTests
    {
        public OracleUnitRawSqlTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddOracleServices()))
        {
        }

        protected override string ExceptedInsertTriggerSqlForMemberArgs
            => "PERFORM func(:NEW.\"BooleanValue\", :NEW.\"DoubleValue\", \"SourceEntities\")";
        
        protected override string ExceptedInsertTriggerSqlForComputedArgs
            => "PERFORM func(:NEW.\"DoubleValue\" + 10)";
        
        protected override string ExceptedInsertTriggerSqlWhenNoArgs
            => "PERFORM func()";
        
        protected override string ExceptedUpdateTriggerSqlForMemberArgs
            => "PERFORM func(:OLD.\"DecimalValue\", :NEW.\"DecimalValue\")";
        
        protected override string ExceptedDeleteTriggerSqlForMemberArgs
            => "PERFORM func(:OLD.\"DecimalValue\", :OLD.\"DoubleValue\")";
    }
}