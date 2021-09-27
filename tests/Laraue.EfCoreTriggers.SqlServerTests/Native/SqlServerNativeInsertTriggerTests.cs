using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerNativeInsertTriggerTests : InsertTests
    {
        public SqlServerNativeInsertTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}