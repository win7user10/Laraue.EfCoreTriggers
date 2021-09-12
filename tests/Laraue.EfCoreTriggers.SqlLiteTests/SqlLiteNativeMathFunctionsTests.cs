using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    public class SqlLiteNativeMathFunctionsTests : NativeMathFunctionTests
    {
        public SqlLiteNativeMathFunctionsTests() : base (new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
