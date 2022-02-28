using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreUnitMemberAssignmentTests : BaseMemberAssignmentUnitTests
    {
        public PostgreUnitMemberAssignmentTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddPostgreSqlServices()))
        {
        }
        
        public override string ExceptedEnumValueSql => "INSERT INTO destination_entities (\"enum_value\") SELECT NEW.enum_value;";

        public override string ExceptedDecimalAddSql => "INSERT INTO destination_entities (\"decimal_value\") SELECT NEW.decimal_value + 3;";

        public override string ExceptedDoubleSubSql => "INSERT INTO destination_entities (\"double_value\") SELECT NEW.double_value + 3;";

        public override string ExceptedIntMultiplySql => "INSERT INTO destination_entities (\"int_value\") SELECT NEW.int_value * 2;";

        public override string ExceptedBooleanSql => "INSERT INTO destination_entities (\"boolean_value\") SELECT NEW.boolean_value is false;";

        public override string ExceptedNewGuidSql => "INSERT INTO destination_entities (\"guid_value\") SELECT uuid_generate_v4();";
    }
}
