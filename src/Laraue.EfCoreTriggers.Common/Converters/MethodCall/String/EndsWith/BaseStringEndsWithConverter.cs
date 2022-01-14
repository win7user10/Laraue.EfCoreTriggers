using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith
{
    public abstract class BaseStringEndsWithConverter : BaseStringConverter
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(string.EndsWith);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var argumentSql = visitor.VisitArguments(expression, argumentTypes, visitedMembers)[0];
            var sqlBuilder = visitor.Visit(expression.Object, argumentTypes, visitedMembers);
            return new SqlBuilder(sqlBuilder.AffectedColumns, $"{sqlBuilder} LIKE {BuildEndSql(argumentSql)}")
                .MergeColumnsInfo(argumentSql);
        }

        protected abstract string BuildEndSql(string argumentSql);
    }
}