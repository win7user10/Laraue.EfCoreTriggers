using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native.TriggerTests;
using Xunit;

namespace Laraue.EfCoreTriggers.MySqlTests.Native
{
    [Collection(CollectionNames.MySql)]
    public class MySqlNativeInsertTriggerTests : InsertTests
    {
        public MySqlNativeInsertTriggerTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}