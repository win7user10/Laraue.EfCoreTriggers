using System.Reflection;

namespace Laraue.Linq2Triggers.Core.SqlGeneration
{
    public sealed record KeyInfo(MemberInfo PrincipalKey, MemberInfo ForeignKey);
}