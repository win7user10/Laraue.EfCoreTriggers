using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.MySqlTests
{
    public class MySqlNativeMemberAssignmentFunctionsTests : NativeMemberAssignmentFunctionsTests
    {
        public MySqlNativeMemberAssignmentFunctionsTests() : base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
