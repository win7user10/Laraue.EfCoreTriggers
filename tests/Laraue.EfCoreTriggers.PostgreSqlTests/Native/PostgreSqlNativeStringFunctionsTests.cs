using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native
{
    public class PostgreSqlNativeStringFunctionsTests : NativeStringFunctionTests
    {
        public PostgreSqlNativeStringFunctionsTests() : base ( new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
