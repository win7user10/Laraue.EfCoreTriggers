using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    public class SqlServerNativeMathFunctionsTests : NativeMathFunctionTests
    {
        public SqlServerNativeMathFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
