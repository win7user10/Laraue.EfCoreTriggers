using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    [Collection("SqlServerNativeTests")]
    public class SqlServerNativeTests : BaseNativeTests
    {
        public SqlServerNativeTests() : base(new ContextFactory().CreateDbContext())
        {
        }

        protected override void InitializeDbContext()
        {
            DbContext.Database.Migrate();
        }
    }
}