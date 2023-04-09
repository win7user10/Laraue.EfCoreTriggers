using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    public sealed record KeyInfo(MemberInfo PrincipalKey, MemberInfo ForeignKey);
}