using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    [Collection("SqlLiteNativeTests")]
    public class SqlLiteNativeTests : BaseNativeTests
    {
        public SqlLiteNativeTests() : base(new ContextFactory().CreateDbContext())
        {
            DbContext.Database.Migrate();
        }
    }
}