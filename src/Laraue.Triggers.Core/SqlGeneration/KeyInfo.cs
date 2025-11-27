using System.Reflection;

namespace Laraue.Triggers.Core.SqlGeneration
{
    public sealed record KeyInfo(MemberInfo PrincipalKey, MemberInfo ForeignKey);
}