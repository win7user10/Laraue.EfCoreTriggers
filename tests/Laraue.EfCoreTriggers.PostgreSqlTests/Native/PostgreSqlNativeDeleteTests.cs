using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlNativeDeleteTests: DeleteTests
    {
        public PostgreSqlNativeDeleteTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}