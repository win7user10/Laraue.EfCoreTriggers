using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerNativeDeleteTriggerTests : DeleteTests
    {
        public SqlServerNativeDeleteTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}