using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.MySqlTests.Native
{
    [Collection(CollectionNames.MySql)]
    public class MySqlNativeStringFunctionsTests : NativeStringFunctionTests
    {
        public MySqlNativeStringFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
