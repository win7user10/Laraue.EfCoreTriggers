using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlServerTests
{
    public class SqlServerNativeMemberAssignmentFunctionsTests : NativeMemberAssignmentFunctionsTests
    {
        public SqlServerNativeMemberAssignmentFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
