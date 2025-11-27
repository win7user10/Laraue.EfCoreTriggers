using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Abstractions;

namespace Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors
{
    /// <summary>
    /// Visitor for the <see cref="ITriggerAction"/>. 
    /// </summary>
    /// <typeparam name="TTriggerAction"></typeparam>
    public interface ITriggerActionVisitor<in TTriggerAction>
        where TTriggerAction : ITriggerAction
    {
        /// <summary>
        /// Visit <see cref="ITriggerAction"/> and return new SQL for it.
        /// </summary>
        /// <param name="triggerAction"></param>
        /// <param name="visitedMembers"></param>
        /// <returns></returns>
        SqlBuilder Visit(TTriggerAction triggerAction, VisitedMembers visitedMembers);
    }
}