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
        public override string ExceptedConcatSql => "INSERT INTO transactions_mirror (description) VALUES (@NewDescription + 'abc');";

        public override string ExceptedStringLowerSql => "INSERT INTO transactions_mirror (description) VALUES (LOWER(@NewDescription));";

        public override string ExceptedStringUpperSql => "INSERT INTO transactions_mirror (description) VALUES (UPPER(@NewDescription));";

        public override string ExceptedStringTrimSql => "INSERT INTO transactions_mirror (description) VALUES (TRIM(@NewDescription));";

        public override string ExceptedContainsSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (CHARINDEX(@NewDescription, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (@NewDescription LIKE ('%' + 'abc'));";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO transactions_mirror (is_veryfied) VALUES (@NewDescription IS NULL OR @NewDescription = '');";

    }
}
