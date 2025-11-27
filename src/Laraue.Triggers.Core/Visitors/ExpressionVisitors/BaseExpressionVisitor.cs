using System.Linq.Expressions;
using Laraue.Triggers.Core.SqlGeneration;

namespace Laraue.Triggers.Core.Visitors.ExpressionVisitors
{
    public abstract class BaseExpressionVisitor<TExpression> : IExpressionVisitor<TExpression>
        where TExpression : Expression 
    {
        public abstract SqlBuilder Visit(
            TExpression expression,
            VisitedMembers visitedMembers);
    }
}