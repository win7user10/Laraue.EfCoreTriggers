using System.Collections.Generic;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.v2;

public class VisitedMembers : Dictionary<ArgumentType, HashSet<MemberInfo>>
{
    public void AddMember(ArgumentType argumentType, MemberInfo member)
    {
        if (!ContainsKey(argumentType))
        {
            this[argumentType] = new HashSet<MemberInfo>();
        }

        this[argumentType].Add(member);
    }
}