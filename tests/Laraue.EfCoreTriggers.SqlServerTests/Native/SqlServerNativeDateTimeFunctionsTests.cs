using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    [Collection(CollectionNames.SqlServer)]
    public class SqlServerNativeDateTimeFunctionsTests : NativeDateTimeFunctionTests
    {
        public SqlServerNativeDateTimeFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}

        protected override bool DateReturnsOnlyInUtc => false;
    }
}
