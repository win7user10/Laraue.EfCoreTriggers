using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;

namespace Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors
{
    public class LambdaExpressionVisitor : BaseExpressionVisitor<LambdaExpression>
    {
        private readonly IExpressionVisitorFactory _factory;

        public LambdaExpressionVisitor(IExpressionVisitorFactory factory)
        {
            _factory = factory;
        }

        public override SqlBuilder Visit(LambdaExpression expression, VisitedMembers visitedMembers)
        {
            return _factory.Visit(expression.Body, visitedMembers);
        }
    }
}