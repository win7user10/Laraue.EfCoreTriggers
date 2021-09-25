using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native
{
    public class SqlLiteNativeStringFunctionsTests : NativeStringFunctionTests
    {
        public SqlLiteNativeStringFunctionsTests() : base (new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
