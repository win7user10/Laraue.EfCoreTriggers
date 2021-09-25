using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native
{
    public class PostgreSqlNativeInsertTests : InsertTests
    {
        public PostgreSqlNativeInsertTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}