using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    public class SqlServerNativeStringFunctionsTests : NativeStringFunctionTests
    {
        public SqlServerNativeStringFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
