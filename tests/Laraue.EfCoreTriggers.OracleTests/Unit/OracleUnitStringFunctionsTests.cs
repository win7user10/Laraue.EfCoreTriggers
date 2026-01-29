using Laraue.EfCoreTriggers.Oracle.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.OracleTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class OracleUnitStringFunctionsTests : UnitStringFunctionsTests
    {
        public OracleUnitStringFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddOracleServices()))
        {
        }

        protected override string ExceptedConcatSql
            => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT CONCAT(:NEW.\"StringField\", N'abc');";

        protected override string ExceptedStringLowerSql
            => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT LOWER(:NEW.\"StringField\");";

        protected override string ExceptedStringUpperSql
            => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT UPPER(:NEW.\"StringField\");";

        protected override string ExceptedStringTrimSql
            => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT TRIM(:NEW.\"StringField\");";

        protected override string ExceptedContainsSql
            => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT CASE WHEN INSTR(:NEW.\"StringField\", N'abc') > 0 THEN 1 ELSE 0 END;";

        protected override string ExceptedEndsWithSql
            => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT CASE WHEN :NEW.\"StringField\" LIKE CONCAT('%', N'abc') THEN 1 ELSE 0 END;";

        protected override string ExceptedIsNullOrEmptySql
            => "INSERT INTO \"DestinationEntities\" (\"BooleanValue\") SELECT CASE WHEN :NEW.\"StringField\" IS NULL OR :NEW.\"StringField\" = N'' THEN 1 ELSE 0 END;";

        protected override string ExceptedCoalesceStringSql
            => "INSERT INTO \"DestinationEntities\" (\"StringField\") SELECT COALESCE(:NEW.\"StringField\", N'John');";
    }
}
