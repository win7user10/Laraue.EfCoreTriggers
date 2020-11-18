using Laraue.EfCoreTriggers.Tests;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Xunit;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    [Collection("SqlLiteNativeTests")]
    public class SqlLiteNativeTests : BaseNativeTests
    {
        public SqlLiteNativeTests() : base(new ContextFactory().CreateDbContext())
        {
        }

        protected override void InitializeDbContext()
        {
            DbContext.Database.Migrate();

            var contextState = DbContext.Database.GetDbConnection().State;
            if (contextState == ConnectionState.Closed)
                DbContext.Database.OpenConnection();

            DbContext.Database.EnsureCreated();
        }
    }
}