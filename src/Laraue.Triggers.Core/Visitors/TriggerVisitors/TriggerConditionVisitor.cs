using System.Linq.Expressions;
using Laraue.Triggers.Core.SqlGeneration;
using Laraue.Triggers.Core.TriggerBuilders.Actions;
using Laraue.Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Triggers.Core.Visitors.TriggerVisitors
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