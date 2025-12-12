using System.Reflection;
using Laraue.Linq2Triggers.Core.SqlGeneration;

namespace Laraue.Linq2Triggers.Core.Extensions;

public static class MemberInfoExtensions
{
    public static VisitedMemberInfo ToVisitedMemberInfo(this MemberInfo source)
    {
        return new VisitedMemberInfo(source.ReflectedType!, source.Name);
    }
}