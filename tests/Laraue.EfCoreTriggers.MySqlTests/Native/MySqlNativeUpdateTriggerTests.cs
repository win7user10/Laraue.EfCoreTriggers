using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.MySqlTests.Native
{
    public class MySqlNativeUpdateTriggerTests : UpdateTests
    {
        public MySqlNativeUpdateTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}