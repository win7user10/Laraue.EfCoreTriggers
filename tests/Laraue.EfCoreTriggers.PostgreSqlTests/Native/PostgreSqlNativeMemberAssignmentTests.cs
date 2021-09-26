using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Native
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlNativeMemberAssignmentTests : NativeMemberAssignmentFunctionsTests
    {
        public PostgreSqlNativeMemberAssignmentTests(): base(new ContextOptionsFactory<DynamicDbContext>(), setupModelBuilder:
            builder =>
            {
                builder.HasPostgresExtension("uuid-ossp");
            }){}
    }
}
