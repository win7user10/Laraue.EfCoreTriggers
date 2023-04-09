using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;

public class TriggerConditionVisitor : ITriggerActionVisitor<TriggerCondition>
{
    private readonly IExpressionVisitorFactory _visitorFactory;

    public TriggerConditionVisitor(IExpressionVisitorFactory visitorFactory)
    {
        _visitorFactory = visitorFactory;
    }

    public SqlBuilder Visit(TriggerCondition triggerAction, VisitedMembers visitedMembers)
    {
        var conditionBody = triggerAction.Condition.Body;
        return conditionBody switch
        {
            MemberExpression memberExpression => _visitorFactory.Visit(Expression.IsTrue(memberExpression), visitedMembers),
            _ => _visitorFactory.Visit(conditionBody, visitedMembers),
        };
    }
    
    public SqlBuilder GetConditionStatementSql(TriggerCondition condition, VisitedMembers visitedMembers)
    {
        var binaryExpressionSql = _visitorFactory.Visit((BinaryExpression)condition.Condition.Body, visitedMembers);
        
        return SqlBuilder.FromString("WHERE ")
            .Append(binaryExpressionSql);
    }
}