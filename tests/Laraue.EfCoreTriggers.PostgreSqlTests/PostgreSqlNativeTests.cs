using Laraue.EfCoreTriggers.SqlTests;
using Laraue.EfCoreTriggers.Tests.NativeTests;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    [Collection("PostgreSqlNativeTests")]
    public class PostgreSqlNativeTests : BaseNativeTests
    {
        public PostgreSqlNativeTests() : base(new ContextFactory().CreatePgDbContext())
        {
        }
    }
}