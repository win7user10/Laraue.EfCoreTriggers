using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.Services;

public class KeyInfo
{
    public MemberInfo PrincipalKey { get; set; }

    public MemberInfo ForeignKey { get; set; }
}