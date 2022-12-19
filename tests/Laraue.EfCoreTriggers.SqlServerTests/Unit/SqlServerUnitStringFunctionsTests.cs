using Laraue.EfCoreTriggers.SqlServer.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitStringFunctionsTests : UnitStringFunctionsTests
    {
        public SqlServerUnitStringFunctionsTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddSqlServerServices()))
        {
        }

        protected override string ExceptedConcatSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT @NewStringField + 'abc';";

        protected override string ExceptedStringLowerSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT LOWER(@NewStringField);";

        protected override string ExceptedStringUpperSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT UPPER(@NewStringField);";

        protected override string ExceptedStringTrimSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT LTRIM(RTRIM(@NewStringField));";

        protected override string ExceptedContainsSql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT CASE WHEN CHARINDEX('abc', @NewStringField) > 0 THEN 1 ELSE 0 END;";

        protected override string ExceptedEndsWithSql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT CASE WHEN @NewStringField LIKE ('%' + 'abc') THEN 1 ELSE 0 END;";

        protected override string ExceptedIsNullOrEmptySql => "INSERT INTO \"destination_entities\" (\"boolean_value\") SELECT CASE WHEN @NewStringField IS NULL OR @NewStringField = '' THEN 1 ELSE 0 END;";

        protected override string ExceptedCoalesceStringSql => "INSERT INTO \"destination_entities\" (\"string_field\") SELECT COALESCE(@NewStringField, 'John');";
    }
}
