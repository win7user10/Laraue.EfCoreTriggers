using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.OracleTests.Native
{
    [Collection(CollectionNames.Oracle)]
    public class OracleNativeDeleteTriggerTests : DeleteTests
    {
        public OracleNativeDeleteTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}