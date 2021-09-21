using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Native;

namespace Laraue.EfCoreTriggers.SqlLiteTests
{
    public class SqlLiteNativeMemberAssignmentTests : NativeMemberAssignmentFunctionsTests
    {
        public SqlLiteNativeMemberAssignmentTests(): base(new ContextOptionsFactory<DynamicDbContext>()){}
    }
}
