using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.Services;

public record KeyInfo
{
    public MemberInfo PrincipalKey { get; init; }

    public MemberInfo ForeignKey { get; init; }
}