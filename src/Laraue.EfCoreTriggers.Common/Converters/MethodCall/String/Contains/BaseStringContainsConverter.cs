using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Internal;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public abstract class BaseStringContainsConverter : BaseStringConverter
    {
        /// <inheritdoc />
        protected override string MethodName => nameof(string.Contains);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(
            IExpressionVisitor visitor,
            MethodCallExpression expression,
            ArgumentTypes argumentTypes,
            VisitedMembers visitedMembers)
        {
            var expressionToFindSql = visitor.VisitArguments(expression, argumentTypes, visitedMembers)[0];
            var expressionToSearchSql = visitor.Visit(expression.Object, argumentTypes, visitedMembers);
            
            return new SqlBuilder(expressionToFindSql.AffectedColumns, CombineSql(expressionToSearchSql, expressionToFindSql))
                .MergeColumnsInfo(expressionToSearchSql);
        }

        protected abstract string CombineSql(string expressionToSearchSql, string expressionToFindSql);
    }
}