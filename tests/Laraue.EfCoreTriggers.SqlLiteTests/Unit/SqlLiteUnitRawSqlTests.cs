using Laraue.EfCoreTriggers.SqlLite.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Unit
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteUnitRawSqlTests : UnitRawSqlTests
    {
        public SqlLiteUnitRawSqlTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqliteServices()))
        {
        }

        protected override string ExceptedInsertTriggerSqlForMemberArgs => "PERFORM func(NEW.\"boolean_value\", NEW.\"double_value\", \"source_entities\")";
        protected override string ExceptedInsertTriggerSqlForComputedArgs => "PERFORM func(NEW.\"double_value\" + 10)";
        protected override string ExceptedInsertTriggerSqlWhenNoArgs => "PERFORM func()";
        protected override string ExceptedUpdateTriggerSqlForMemberArgs => "PERFORM func(OLD.\"decimal_value\", NEW.\"decimal_value\")";
        protected override string ExceptedDeleteTriggerSqlForMemberArgs => "PERFORM func(OLD.\"decimal_value\", OLD.\"double_value\")";
    }
}