using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    [Collection("PostgreSqlNativeTests")]
    public class PostgreSqlNativeTests : BaseNativeTests
    {
        public PostgreSqlNativeTests() : base(new ContextFactory().CreateDbContext())
        {
        }
    }
}