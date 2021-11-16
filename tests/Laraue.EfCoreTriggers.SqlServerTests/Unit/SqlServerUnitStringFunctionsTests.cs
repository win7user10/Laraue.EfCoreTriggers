using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.SqlServerTests.Unit
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerUnitStringFunctionsTests : UnitStringFunctionsTests
    {
        public SqlServerUnitStringFunctionsTests() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedConcatSql => "INSERT INTO destination_entities (\"string_field\") VALUES (@NewStringField + 'abc');";

        public override string ExceptedStringLowerSql => "INSERT INTO destination_entities (\"string_field\") VALUES (LOWER(@NewStringField));";

        public override string ExceptedStringUpperSql => "INSERT INTO destination_entities (\"string_field\") VALUES (UPPER(@NewStringField));";

        public override string ExceptedStringTrimSql => "INSERT INTO destination_entities (\"string_field\") VALUES (LTRIM(RTRIM(@NewStringField)));";

        public override string ExceptedContainsSql => "INSERT INTO destination_entities (\"boolean_value\") VALUES (CASE WHEN CHARINDEX('abc', @NewStringField) > 0 THEN 1 ELSE 0 END);";

        public override string ExceptedEndsWithSql => "INSERT INTO destination_entities (\"boolean_value\") VALUES (CASE WHEN @NewStringField LIKE ('%' + 'abc') THEN 1 ELSE 0 END);";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO destination_entities (\"boolean_value\") VALUES (CASE WHEN @NewStringField IS NULL OR @NewStringField = '' THEN 1 ELSE 0 END);";

    }
}
