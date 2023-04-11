using System.Linq;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;

namespace Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors
{
    public abstract class BaseTriggerActionsGroupVisitor : ITriggerActionVisitor<TriggerActionsGroup>
    {
        private readonly ITriggerActionVisitorFactory _factory;

        protected BaseTriggerActionsGroupVisitor(ITriggerActionVisitorFactory factory)
        {
            _factory = factory;
        }
    
        public SqlBuilder Visit(TriggerActionsGroup triggerActionsGroup, VisitedMembers visitedMembers)
        {
            var actionsSql = triggerActionsGroup.ActionExpressions
                .Select(action => _factory.Visit(action, visitedMembers))
                .ToArray();
        
            var conditionsSql = triggerActionsGroup.ActionConditions
                .Select(actionCondition => _factory.Visit(actionCondition, visitedMembers))
                .ToArray();

            return GetActionSql(actionsSql, conditionsSql);
        }

        protected abstract SqlBuilder GetActionSql(SqlBuilder[] actionsSql, SqlBuilder[] conditionsSql);
    }
}