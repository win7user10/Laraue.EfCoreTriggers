using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteNativeMathFunctionsTests : NativeMathFunctionTests
    {
        public SqlLiteNativeMathFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>(), context =>
        {
            var connection = context.Database.GetDbConnection() as SqliteConnection;
            connection.LoadExtension("Extensions/math.dll");
        }){}
    }
}
