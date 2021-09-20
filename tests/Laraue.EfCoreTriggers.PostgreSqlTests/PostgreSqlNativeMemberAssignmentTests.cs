using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    public class PostgreSqlNativeMemberAssignmentTests : NativeMemberAssigmentFunctionsTests
    {
        public PostgreSqlNativeMemberAssignmentTests(): base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
