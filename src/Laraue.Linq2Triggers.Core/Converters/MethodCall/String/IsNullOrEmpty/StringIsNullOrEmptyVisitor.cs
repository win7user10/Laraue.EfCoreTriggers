using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;

namespace Laraue.Linq2Triggers.Core.Converters.MethodCall.String.IsNullOrEmpty
{
    public class StringIsNullOrEmptyVisitor : BaseStringVisitor
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(string.IsNullOrEmpty);

        /// <inheritdoc />
        public StringIsNullOrEmptyVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            var isNullExpression = Expression.Equal(argument, Expression.Constant(null));
            var isEmptyExpression = Expression.Equal(argument, Expression.Constant(string.Empty));
            var isNullOrEmptyExpression = Expression.OrElse(isNullExpression, isEmptyExpression);
            return VisitorFactory.Visit(isNullOrEmptyExpression, visitedMembers);
        }
    }
}