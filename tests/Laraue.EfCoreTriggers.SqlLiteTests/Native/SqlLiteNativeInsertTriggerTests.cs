using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native
{
    public class SqlLiteNativeInsertTriggerTests : InsertTests
    {
        public SqlLiteNativeInsertTriggerTests(): base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}