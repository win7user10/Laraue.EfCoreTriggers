using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors
{
    public abstract class BaseExpressionVisitor<TExpression> : IExpressionVisitor<TExpression>
        where TExpression : Expression 
    {
        public abstract SqlBuilder Visit(
            TExpression expression,
            VisitedMembers visitedMembers);
    }
}