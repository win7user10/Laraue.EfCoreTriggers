using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native;

[Collection(CollectionNames.PostgreSql)]
public class PostgreSqlNativeEnumerableFunctionsTests : NativeEnumerableFunctionsTests
{
    public PostgreSqlNativeEnumerableFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
}