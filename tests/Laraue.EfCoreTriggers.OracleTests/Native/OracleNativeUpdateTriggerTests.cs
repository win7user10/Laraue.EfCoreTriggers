using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.OracleTests.Native
{
    [Collection(CollectionNames.Oracle)]
    public class OracleNativeUpdateTriggerTests : UpdateTests
    {
        public OracleNativeUpdateTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}