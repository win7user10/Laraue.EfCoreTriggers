using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlLiteTests.Native
{
    public class SqlLiteNativeMemberAssignmentTests : NativeMemberAssignmentFunctionsTests
    {
        public SqlLiteNativeMemberAssignmentTests(): base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
