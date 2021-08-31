using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laraue.EfCoreTriggers.SqlServer;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    [UnitTest]
    public class SqlServerUnitStringFunctionsTests : UnitStringFunctionsTests
    {
        public SqlServerUnitStringFunctionsTests() : base(new SqlServerProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedConcatSql => "INSERT INTO destination_entities (string_field) VALUES (@NewStringField + 'abc');";

        public override string ExceptedStringLowerSql => "INSERT INTO destination_entities (string_field) VALUES (LOWER(@NewStringField));";

        public override string ExceptedStringUpperSql => "INSERT INTO destination_entities (string_field) VALUES (UPPER(@NewStringField));";

        public override string ExceptedStringTrimSql => "INSERT INTO destination_entities (string_field) VALUES (TRIM(@NewStringField));";

        public override string ExceptedContainsSql => "INSERT INTO destination_entities (boolean_value) VALUES (CHARINDEX(@NewStringField, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO destination_entities (boolean_value) VALUES (@NewStringField LIKE ('%' + 'abc'));";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO destination_entities (boolean_value) VALUES (@NewStringField IS NULL OR @NewStringField = '');";

    }
}
