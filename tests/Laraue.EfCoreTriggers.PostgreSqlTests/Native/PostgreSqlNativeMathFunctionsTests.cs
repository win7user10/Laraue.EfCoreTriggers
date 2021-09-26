using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlNativeMathFunctionsTests : NativeMathFunctionTests
    {
        public PostgreSqlNativeMathFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}