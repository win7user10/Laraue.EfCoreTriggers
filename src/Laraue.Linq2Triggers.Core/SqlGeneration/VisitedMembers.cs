using System;
using System.Collections.Generic;
using Laraue.Linq2Triggers.Core.TriggerBuilders;

namespace Laraue.Linq2Triggers.Core.SqlGeneration
{
    /// <summary>
    /// Dictionary with all visited members while SQL generation. 
    /// </summary>
    public class VisitedMembers : Dictionary<ArgumentType, HashSet<VisitedMemberInfo>>
    {
        /// <summary>
        /// Add new visited member.
        /// </summary>
        /// <param name="argumentType">Member prefix.</param>
        /// <param name="member">Info about member.</param>
        public void AddMember(ArgumentType argumentType, VisitedMemberInfo member)
        {
            if (!ContainsKey(argumentType))
            {
                this[argumentType] = new HashSet<VisitedMemberInfo>();
            }

            this[argumentType].Add(member);
        }
    }

    /// <summary>
    /// Contains information about used property (Defined or Shadow) in trigger.
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="MemberName"></param>
    public record VisitedMemberInfo(Type Type, string MemberName);
}