using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.Services;

public record KeyInfo
{
    public MemberInfo PrincipalKey { get; set; }

    public MemberInfo ForeignKey { get; set; }
}