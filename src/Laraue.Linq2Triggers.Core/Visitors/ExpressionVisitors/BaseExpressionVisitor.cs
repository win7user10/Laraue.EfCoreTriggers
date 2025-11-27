using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;

namespace Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors
{
    public abstract class BaseExpressionVisitor<TExpression> : IExpressionVisitor<TExpression>
        where TExpression : Expression 
    {
        public abstract SqlBuilder Visit(
            TExpression expression,
            VisitedMembers visitedMembers);
    }
}