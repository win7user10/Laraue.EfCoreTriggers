using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    public class SqlServerNativeInsertTriggerTests : InsertTests
    {
        public SqlServerNativeInsertTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}