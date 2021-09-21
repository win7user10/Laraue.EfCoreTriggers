using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;
using Microsoft.EntityFrameworkCore;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    public class PostgreSqlNativeMemberAssignmentTests : NativeMemberAssignmentFunctionsTests
    {
        public PostgreSqlNativeMemberAssignmentTests(): base(new ContextOptionsFactory<DynamicDbContext>(), setupModelBuilder:
            builder =>
            {
                builder.HasPostgresExtension("uuid-ossp");
            }){}
    }
}
