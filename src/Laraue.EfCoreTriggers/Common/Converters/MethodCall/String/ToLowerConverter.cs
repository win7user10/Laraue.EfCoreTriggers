using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Builders.Providers;

namespace Laraue.EfCoreTriggers.Common.Converters.ExpressionCall.String
{
    public class ToLowerConverter : BaseStringConverter
    {
        /// <inheritdoc />
        public override string MethodName => nameof(string.ToLower);

        /// <inheritdoc />
        public override SqlBuilder BuildSql(BaseExpressionProvider provider, MethodCallExpression expression, Dictionary<string, ArgumentType> argumentTypes)
        {
            var sqlBuilder = provider.GetExpressionSql(expression.Object, argumentTypes);
            return new(sqlBuilder.AffectedColumns, $"LOWER({sqlBuilder})");
        }
    }
}