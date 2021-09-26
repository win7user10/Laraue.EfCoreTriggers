using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native
{
    public class SqlLiteNativeUpdateTriggerTests : UpdateTests
    {
        public SqlLiteNativeUpdateTriggerTests(): base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}