using Laraue.EfCoreTriggers.Tests;
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