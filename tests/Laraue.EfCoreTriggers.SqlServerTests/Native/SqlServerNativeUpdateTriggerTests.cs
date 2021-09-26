using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerNativeUpdateTriggerTests : UpdateTests
    {
        public SqlServerNativeUpdateTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}