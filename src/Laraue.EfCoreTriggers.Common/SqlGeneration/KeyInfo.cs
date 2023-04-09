using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    public record KeyInfo
    {
        public MemberInfo PrincipalKey { get; set; }

        public MemberInfo ForeignKey { get; set; }
    }
}