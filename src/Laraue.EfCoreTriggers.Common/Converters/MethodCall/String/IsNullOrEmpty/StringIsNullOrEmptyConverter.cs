using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.IsNullOrEmpty
{
    public class StringIsNullOrEmptyConverter : BaseStringConverter
    {
        protected override string MethodName => nameof(string.IsNullOrEmpty);

        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argument = expression.Arguments[0];
            var isNullExpression = Expression.Equal(argument, Expression.Constant(null));
            var isEmptyExpression = Expression.Equal(argument, Expression.Constant(string.Empty));
            var isNullOrEmptyExpression = Expression.OrElse(isNullExpression, isEmptyExpression);
            return visitor.Visit(isNullOrEmptyExpression, argumentTypes, visitedMembers);
        }
    }
}