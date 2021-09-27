using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteNativeInsertTriggerTests : InsertTests
    {
        public SqlLiteNativeInsertTriggerTests(): base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}