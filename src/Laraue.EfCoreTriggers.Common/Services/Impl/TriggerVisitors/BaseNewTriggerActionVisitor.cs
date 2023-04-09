using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

public abstract class BaseNewTriggerActionVisitor : ITriggerActionVisitor<NewTriggerAction>
{
    private readonly ITriggerActionVisitorFactory _factory;

    protected BaseNewTriggerActionVisitor(ITriggerActionVisitorFactory factory)
    {
        _factory = factory;
    }
    
    public SqlBuilder Visit(NewTriggerAction triggerAction, VisitedMembers visitedMembers)
    {
        var actionsSql = triggerAction.ActionExpressions
            .Select(action => _factory.Visit(action, visitedMembers))
            .ToArray();
        
        var conditionsSql = triggerAction.ActionConditions
            .Select(actionCondition => _factory.Visit(actionCondition, visitedMembers))
            .ToArray();

        return GetActionSql(actionsSql, conditionsSql);
    }

    protected abstract SqlBuilder GetActionSql(SqlBuilder[] actionsSql, SqlBuilder[] conditionsSql);
}