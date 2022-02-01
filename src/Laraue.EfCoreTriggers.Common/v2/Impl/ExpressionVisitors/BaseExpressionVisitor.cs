using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

public abstract class BaseExpressionVisitor<TExpression> : IExpressionVisitor<TExpression>
    where TExpression : Expression 
{
    public abstract SqlBuilder Visit(
        TExpression expression,
        ArgumentTypes argumentTypes,
        VisitedMembers visitedMembers);
}