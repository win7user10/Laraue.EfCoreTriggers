using Laraue.EfCoreTriggers.Tests.Tests;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    [Collection("SqlLiteNativeTests")]
    public class SqlLiteNativeTests : BaseNativeTests
    {
        public SqlLiteNativeTests() : base(new ContextFactory().CreateDbContext())
        {
        }
    }
}