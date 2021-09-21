using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    public class MySqlNativeMathFunctionsTests : NativeMathFunctionTests
    {
        public MySqlNativeMathFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
