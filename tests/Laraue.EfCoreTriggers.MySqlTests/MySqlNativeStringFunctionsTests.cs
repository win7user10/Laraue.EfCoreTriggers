using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    public class MySqlNativeStringFunctionsTests : NativeStringFunctionTests
    {
        public MySqlNativeStringFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
