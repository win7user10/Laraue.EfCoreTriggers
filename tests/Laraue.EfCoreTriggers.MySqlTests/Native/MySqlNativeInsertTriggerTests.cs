using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.MySqlTests.Native
{
    public class MySqlNativeInsertTriggerTests : InsertTests
    {
        public MySqlNativeInsertTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}