using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.IsNullOrEmpty
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
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            var isNullExpression = Expression.Equal(argument, Expression.Constant(null));
            var isEmptyExpression = Expression.Equal(argument, Expression.Constant(string.Empty));
            var isNullOrEmptyExpression = Expression.OrElse(isNullExpression, isEmptyExpression);
            return VisitorFactory.Visit(isNullOrEmptyExpression, argumentTypes, visitedMembers);
        }
    }
}