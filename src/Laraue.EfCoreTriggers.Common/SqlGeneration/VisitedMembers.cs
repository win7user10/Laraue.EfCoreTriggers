using System.Collections.Generic;
using System.Reflection;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.SqlGeneration
{
    /// <summary>
    /// Dictionary with all visited members while SQL generation. 
    /// </summary>
    public class VisitedMembers : Dictionary<ArgumentType, HashSet<MemberInfo>>
    {
        /// <summary>
        /// Add new visited member.
        /// </summary>
        /// <param name="argumentType">Member prefix.</param>
        /// <param name="member">Info about member.</param>
        public void AddMember(ArgumentType argumentType, MemberInfo member)
        {
            if (!ContainsKey(argumentType))
            {
                this[argumentType] = new HashSet<MemberInfo>();
            }

            this[argumentType].Add(member);
        }
    }
}