using Laraue.EfCoreTriggers.Tests;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    [Collection("SqlServerNativeTests")]
    public class SqlServerNativeTests : BaseNativeTests
    {
        public SqlServerNativeTests() : base(new ContextFactory().CreateDbContext())
        {
        }
    }
}