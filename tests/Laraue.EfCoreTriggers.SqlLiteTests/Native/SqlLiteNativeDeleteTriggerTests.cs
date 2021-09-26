using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native
{
    [Collection(CollectionNames.Sqlite)]
    public class SqlLiteNativeDeleteTriggerTests : DeleteTests
    {
        public SqlLiteNativeDeleteTriggerTests(): base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}