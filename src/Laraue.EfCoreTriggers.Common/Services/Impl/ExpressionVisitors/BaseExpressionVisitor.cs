using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

public abstract class BaseExpressionVisitor<TExpression> : IExpressionVisitor<TExpression>
    where TExpression : Expression 
{
    public abstract SqlBuilder Visit(
        TExpression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers);
}