using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    [Collection("PostgreSqlNativeTests")]
    public class PostgreSqlNativeTests : BaseNativeTests
    {
        public PostgreSqlNativeTests() : base(new ContextFactory().CreateDbContext())
        {
        }

        protected override void InitializeDbContext()
        {
            DbContext.Database.Migrate();
        }
    }
}