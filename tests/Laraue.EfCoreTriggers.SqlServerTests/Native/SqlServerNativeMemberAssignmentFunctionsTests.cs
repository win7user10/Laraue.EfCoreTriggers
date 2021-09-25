using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlServerTests.Native
{
    public class SqlServerNativeMemberAssignmentFunctionsTests : NativeMemberAssignmentFunctionsTests
    {
        public SqlServerNativeMemberAssignmentFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
