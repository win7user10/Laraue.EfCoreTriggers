using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlUnitRawSqlTests : UnitRawSqlTests
    {
        public PostgreSqlUnitRawSqlTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddPostgreSqlServices()))
        {
        }

        protected override string ExceptedInsertTriggerSqlForMemberArgs => "PERFORM func(NEW.\"BooleanValue\", NEW.\"DoubleValue\", \"SourceEntities\")";
        protected override string ExceptedInsertTriggerSqlForComputedArgs => "PERFORM func(NEW.\"DoubleValue\" + 10)";
        protected override string ExceptedInsertTriggerSqlWhenNoArgs => "PERFORM func()";
        protected override string ExceptedUpdateTriggerSqlForMemberArgs => "PERFORM func(OLD.\"DecimalValue\", NEW.\"DecimalValue\")";
        protected override string ExceptedDeleteTriggerSqlForMemberArgs => "PERFORM func(OLD.\"DecimalValue\", OLD.\"DoubleValue\")";
    }
}