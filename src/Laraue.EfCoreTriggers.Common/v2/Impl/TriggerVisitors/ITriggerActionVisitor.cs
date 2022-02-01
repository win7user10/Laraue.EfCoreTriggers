using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public interface ITriggerActionVisitor<in TTriggerAction>
    where TTriggerAction : ITriggerAction
{
    SqlBuilder Visit(TTriggerAction triggerAction, VisitedMembers visitedMembers);
}