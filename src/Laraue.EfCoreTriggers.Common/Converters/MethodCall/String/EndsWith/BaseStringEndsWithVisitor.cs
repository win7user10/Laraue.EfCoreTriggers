using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
{
    public abstract class BaseStringEndsWithVisitor : BaseStringVisitor
    {
        protected BaseStringEndsWithVisitor(IExpressionVisitorFactory visitorFactory)
            : base(visitorFactory)
        {
        }
        
        /// <inheritdoc />
        protected override string MethodName => nameof(string.EndsWith);

        /// <inheritdoc />
        public override SqlBuilder Visit(
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argumentSql = VisitorFactory.VisitArguments(expression, argumentTypes, visitedMembers)[0];
            var sqlBuilder = VisitorFactory.Visit(expression.Object, argumentTypes, visitedMembers);
            return SqlBuilder.FromString($"{sqlBuilder} LIKE {BuildEndSql(argumentSql)}");
        }

        protected abstract string BuildEndSql(string argumentSql);
    }
}