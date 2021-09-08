using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    public class PostgreSqlNativeMathFunctionsTests : NativeMathFunctionTests
    {
        public PostgreSqlNativeMathFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}