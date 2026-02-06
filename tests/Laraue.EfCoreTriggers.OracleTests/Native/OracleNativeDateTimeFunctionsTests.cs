using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.OracleTests.Native
{
    [Collection(CollectionNames.Oracle)]
    public class OracleNativeDateTimeFunctionsTests : NativeDateTimeFunctionTests
    {
        public OracleNativeDateTimeFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}

        protected override bool DateReturnsOnlyInUtc => false;
    }
}
