using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlNativeStringFunctionsTests : NativeStringFunctionTests
    {
        public PostgreSqlNativeStringFunctionsTests() : base ( new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
