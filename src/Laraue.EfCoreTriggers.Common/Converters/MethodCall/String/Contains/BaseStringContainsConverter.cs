using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains
{
    public abstract class BaseStringContainsConverter : BaseStringConverter
    {
        /// <inheritdoc />
        public override string MethodName => nameof(string.Contains);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var expressionToFindSql = provider.GetMethodCallArgumentsSql(expression, argumentTypes)[0];
            var expressionToSearchSql = provider.GetExpressionSql(expression.Object, argumentTypes);
            
            return new SqlBuilder(expressionToFindSql.AffectedColumns, CombineSql(expressionToSearchSql, expressionToFindSql))
                .MergeColumnsInfo(expressionToSearchSql);
        }

        protected abstract string CombineSql(string expressionToSearchSql, string expressionToFindSql);
    }
}