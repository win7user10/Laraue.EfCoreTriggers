using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native
{
    public class PostgreSqlNativeUpdateTests: UpdateTests
    {
        public PostgreSqlNativeUpdateTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}