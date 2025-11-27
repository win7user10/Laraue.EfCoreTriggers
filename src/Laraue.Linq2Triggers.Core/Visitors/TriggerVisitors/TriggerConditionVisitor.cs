using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Actions;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors
{
    public class TriggerConditionVisitor : ITriggerActionVisitor<TriggerCondition>
    {
        private readonly IExpressionVisitorFactory _visitorFactory;

        public TriggerConditionVisitor(IExpressionVisitorFactory visitorFactory)
        {
            _visitorFactory = visitorFactory;
        }

        /// <inheritdoc />
        public SqlBuilder Visit(TriggerCondition triggerAction, VisitedMembers visitedMembers)
        {
            var conditionBody = triggerAction.Predicate.Body;
            return conditionBody switch
            {
                MemberExpression memberExpression => _visitorFactory.Visit(Expression.IsTrue(memberExpression), visitedMembers),
                _ => _visitorFactory.Visit(conditionBody, visitedMembers),
            };
        }
    }
}