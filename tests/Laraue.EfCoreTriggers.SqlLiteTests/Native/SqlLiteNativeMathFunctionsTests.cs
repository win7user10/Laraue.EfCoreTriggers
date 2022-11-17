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
            LoadExtension(context, "math.dll");
        }){}

        private static void LoadExtension(DbContext dbContext, string extensionName)
        {
            var extensionPath = $"Extensions/{extensionName}";
            
#if NET6_0_OR_GREATER
            using var connection = dbContext.Database.GetDbConnection() as SqliteConnection;
            connection!.Open();
            connection.EnableExtensions();
            var comm = connection.CreateCommand();
            comm.CommandText = $"SELECT load_extension('{extensionPath}');";
            comm.ExecuteNonQuery();
            connection.Close();
#else
            using var connection = dbContext.Database.GetDbConnection() as SqliteConnection;
            connection!.LoadExtension(extensionPath);
#endif
        }
    }
}
