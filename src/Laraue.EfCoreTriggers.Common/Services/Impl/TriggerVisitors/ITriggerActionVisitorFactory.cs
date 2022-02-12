using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

/// <summary>
/// Factory to get suitable <see cref="ITriggerAction"/> depending on its type.
/// </summary>
public interface ITriggerActionVisitorFactory
{
    /// <summary>
    /// Initialize suitable <see cref="ITriggerAction"/> and call its
    /// <see cref="ITriggerAction.Visit"/>
    /// </summary>
    /// <param name="triggerAction"></param>
    /// <param name="visitedMembers"></param>
    /// <returns></returns>
    SqlBuilder Visit(ITriggerAction triggerAction, VisitedMembers visitedMembers);
}