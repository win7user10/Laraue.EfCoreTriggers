using System.IO;
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
            LoadExtension(context, "math");
        }){}

        private static void LoadExtension(DbContext dbContext, string extensionName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var extensionPath = Path.Combine(currentDirectory, "Extensions", extensionName);
            
            using var connection = dbContext.Database.GetDbConnection() as SqliteConnection;
            connection!.LoadExtension(extensionPath);
        }
    }
}
