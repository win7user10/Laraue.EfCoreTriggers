using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    public class SqlServerNativeUpdateTriggerTests : UpdateTests
    {
        public SqlServerNativeUpdateTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}