using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    public class SqlLiteNativeMathFunctionsTests : NativeMathFunctionTests
    {
        public SqlLiteNativeMathFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>(), context =>
        {
            var connection = context.Database.GetDbConnection() as SqliteConnection;
            connection.LoadExtension("Extensions/math.dll");
        }){}
    }
}
